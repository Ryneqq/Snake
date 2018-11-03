using UnityEngine;

public class Perception {

    Snake snake;

    public Perception(Snake snake)
    {
        this.snake = snake;
    }

    public Matrix CreatePerception() 
    {
        var perception = new Matrix(10, 1);
        var view       = GetView();

        for (int i = 0; i < 6; i++)
            perception[i,0] = view[i];

        var dir = Map.Direction(snake.Direction());
        perception[6, 0] = dir.x;
        perception[7, 0] = dir.y;

        var dist = DistanceToFood();
        perception[8, 0] = dist.x;
        perception[9, 0] = dist.y;

        ChangePerceptionOfTheFood(perception);

        return perception;
    }

    private double[] GetView()
    {
        var view = new double[6];
        var pos  = this.snake.Head();
        var dir  = this.snake.Direction();
        var next  = Map.Neighbour(pos, dir);

        view[0] = (double)  Map.Neighbour(next, Map.Left(dir)).field;
        view[1] = (double)  next.field;
        view[2] = (double)  Map.Neighbour(next, Map.Right(dir)).field;
        view[3] = (double)  Map.Neighbour(pos, Map.Left(dir)).field;
        view[4] = (double)  pos.field;
        view[5] = (double)  Map.Neighbour(pos, Map.Right(dir)).field;

        return view;
    }

    private Vector2 DistanceToFood() {
        var head = snake.Head();
        var food = snake.food.Position();

        return new Vector2(head.x - food.x, head.y - food.y);
    }

    private void ChangePerceptionOfTheFood(Matrix perception) {
        for(int i = 0; i < perception.rows; i++)
            for(int j = 0; j < perception.cols; j++)
                if(perception[i,j] == 3)
                    perception[i,j] = -1;
    }
}
