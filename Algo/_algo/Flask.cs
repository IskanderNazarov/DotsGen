using System.Collections.Generic;

public class Flask {
    public int[] container;
    public int size;
    public int index = 0;

    public Flask(int size, bool isEmpty, int color) {
        this.size = size;
        container = new int[size];
        if (isEmpty) {
            index = 0;
        }
        else {
            for (int i = 0; i < size; i++) {
                container[i] = color;
                index = size;
            }
        }
    }

    public Flask(int size, int[] array) {
        this.size = size;
        this.container = new int[size];
        int i;
        for (i = 0; i < size; i++) {
            if (array[i] == 0) {
                break;
            }

            container[i] = array[i];
        }

        index = i;
    }

    public Flask(Flask flask) {
        this.size = flask.size;
        container = new int[size];
        index = flask.index;
        for (int i = 0; i < size; i++) {
            container[i] = flask.container[i];
        }
    }

    //--------------------------------------------------------------------------

    
    /*
    public Flask(GStack stack, int size) {
        //first reverse the stack
        var balls = new List<BallType>();
        foreach (var b in stack.ballsStack) {
            balls.Add(b.type);
        }

        balls.Reverse();
        while (balls.Count < size) {
            balls.Add(0);
        }


        this.size = size;
        container = new int[size];
        

        var ind = 0;
        foreach (var b in balls) {
            //Debug.Log("ind = " + ind);
            if (balls[ind] == 0) {
                break;
            }

            container[ind] = (int) balls[ind];
            ind++;
            //Debug.Log("t = " + balls[ind]);
        }

        index = ind;


        //Debug.Log("t =============");
    }
    */
    //--------------------------------------------------------------------------

    public bool isFull() {
        return index >= size;
    }

    public bool isEmpty() {
        return index == 0;
    }

    public int topColor() {
        return container[index];
    }

    public bool canReverseMove() {
        if (index == 0) return false;
        if (index == 1) return true;
        return container[index - 1] == container[index - 2];
    }

    public bool canAdd(int ball) {
        if (isFull()) {
            return false;
        }

        if (isEmpty()) {
            return true;
        }

        return top() == ball;
    }

    public int pop() {
        int ball = container[index - 1];
        container[index - 1] = 0;
        index--;
        return ball;
    }

    public int top() {
        int ball = container[index - 1];
        return ball;
    }

    public void push(int item) {
        //System.out.println("Pushed = " + item);
        container[index++] = item;
    }

    public bool isSolved() {
        if (index < size) return false;
        int color = container[0];
        for (int i = 1; i < size; i++) {
            if (container[i] != color) return false;
        }

        return true;
    }


    private long L;

    public override string ToString() {
        /*
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < size; i++) {
            sb.Append(container[i] + " ");
        }
        return sb.ToString();
        */
        L = 0;
        for (int i = 0; i < size; i++) {
            var l = 1L;
            l = l << container[i];
            L = L | l;
            //sb.Append(container[i] + " ");
        }

        return L.ToString();
    }
}