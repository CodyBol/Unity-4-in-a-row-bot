using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Bot_Marvin : MonoBehaviour {

    /**
     * @var int player chip color of the bot (1 = yellow and 2 = red)
     * @var int opponent chip color of the player (1 = yellow and 2 = red)
     * @var GameObject Referee gameobject of the gamehandler
     * @var int[] position this is going to be filled with the points per possible location on the board
     * @var int bestPosition number of the best position
     */

    public int player = 2;
    public int opponent = 1;
    private GameObject Referee;
    public int[] position;
    private int bestPosition;

    /**
     * finds the game handler
     */
    void Start()
    {
        Referee = GameObject.Find("Referee");
    }

    /**
     * resets the postion array so it doesn't interfers with the next round
     */
    private void restetVar()
    {
        for (int i = 0; i < position.Length; i++)
        {
            position[i] = 0;
        }
    }


    /**
     * get all the points of all locations
     */
    private void scanPositions(int competitor)
    {

        for (int i = 0; i <= 6; i++)
        {

            int height = 0;

            if (Referee.GetComponent<BoardStatus>().field[i, 0] != 0)
            {

                for (int n = 1; n < 5; n++)
                {

                    if (Referee.GetComponent<BoardStatus>().field[i, n] == 0 && Referee.GetComponent<BoardStatus>().field[i, n - 1] != 0)
                    {
                        height = n;
                    }
                }
            }

            if (Referee.GetComponent<BoardStatus>().field[i, 5] != 0)
            {
                position[i] = -200;
            }
            else
            {
                int pos = position[i];

                pos += Referee.GetComponent<BoardCheck>().horizontalCheck(i, height, competitor);
                pos += Referee.GetComponent<BoardCheck>().verticalCheck(i, height, competitor);
                pos += Referee.GetComponent<BoardCheck>().diagonale_1Check(i, height, competitor);
                pos += Referee.GetComponent<BoardCheck>().diagonale_2Check(i, height, competitor);

                position[i] = pos;
            }

        }
    }


    /**
     * check if bot can win this round
     */
    private int checkForWin(int competitor)
    {
        for (int i = 0; i <= 6; i++)
        {

            int height = 0 ;

            if (Referee.GetComponent<BoardStatus>().field[i, 0] != 0)
            {
                for (int n = 1; n <= 5; n++)
                {
                    if (Referee.GetComponent<BoardStatus>().field[i, n] == 0 && Referee.GetComponent<BoardStatus>().field[i, n - 1] != 0)
                    {
                        height = n;
                    }
                }
            }

            if (Referee.GetComponent<BoardStatus>().field[i, 5] == 0)
            {

                if (Referee.GetComponent<BoardCheck>().horizontalCheck(i, height, competitor) >= 3)
                {
                    return i;
                }

                if (Referee.GetComponent<BoardCheck>().verticalCheck(i, height, competitor) >= 3)
                {
                    return i;
                }

                if (Referee.GetComponent<BoardCheck>().diagonale_1Check(i, height, competitor) >= 3)
                {
                    return i;
                }

                if (Referee.GetComponent<BoardCheck>().diagonale_2Check(i, height, competitor) >= 3)
                {
                    return i;
                }
            }
        }

        return 111;
    }

    /**
     * check if the best position gives the competitor a win the next turn
     */
    public void checkWinNext(int competitor)
    {
        for (int i = 0; i <= 6; i++)
        {
            int height = 0 + 1;

            if (Referee.GetComponent<BoardStatus>().field[i, 0] != 0)
            {
                for (int n = 1; n < 5; n++)
                {
                    if (Referee.GetComponent<BoardStatus>().field[i, n] == 0 && Referee.GetComponent<BoardStatus>().field[i, n - 1] != 0)
                    {
                        height = n + 1;
                    }
                }
            }

            if (Referee.GetComponent<BoardStatus>().field[i, 5] == 0)
            {

                if (Referee.GetComponent<BoardCheck>().horizontalCheck(i, height, competitor) >= 3)
                {
                    position[i] = -100;
                }

                if (Referee.GetComponent<BoardCheck>().verticalCheck(i, height, competitor) >= 3)
                {
                    position[i] = -100;
                }

                if (Referee.GetComponent<BoardCheck>().diagonale_1Check(i, height, competitor) >= 3)
                {
                    position[i] = -100;
                }

                if (Referee.GetComponent<BoardCheck>().diagonale_2Check(i, height, competitor) >= 3)
                {
                    position[i] = -100;
                }
            }
        }
    }

    public void calculateBestPosition()
    {
        /**
         * check if bot can win if so give that the highest pro 
         */
        int won = this.checkForWin(player);

        /**
         * check if opponent can win if so give that the highest pro
         */
        int counter = this.checkForWin(opponent);

        /**
         * check if next move will block a win for the bot
         */
        this.checkWinNext(player);

        /**
         * check if next move lets the opponent win
         */
        this.checkWinNext(opponent);

        bool scan = true;

        if (won != 111)
        {
            bestPosition = won;
            scan = false;
        }

        if (counter != 111 && scan == true)
        {
            bestPosition = counter;
            scan = false;
        }

        if (scan == true)
        {

            /**
             * add points to all positions
             */
            this.scanPositions(player);
            this.scanPositions(opponent);


            //TODO clean this mess

            /**
             * get best position
             */
            if (position[0] >= position[1] && position[0] >= position[2] && position[0] >= position[3] && position[0] >= position[4] && position[0] >= position[5] && position[0] >= position[6])
            {
                bestPosition = 0;
            }

            if (position[1] >= position[0] && position[1] >= position[2] && position[1] >= position[3] && position[1] >= position[4] && position[1] >= position[5] && position[1] >= position[6])
            {
                bestPosition = 1;
            }

            if (position[2] >= position[1] && position[2] >= position[0] && position[2] >= position[3] && position[2] >= position[4] && position[2] >= position[5] && position[2] >= position[6])
            {
                bestPosition = 2;
            }

            if (position[3] >= position[1] && position[3] >= position[2] && position[3] >= position[0] && position[3] >= position[4] && position[3] >= position[5] && position[3] >= position[6])
            {
                bestPosition = 3;
            }

            if (position[4] >= position[1] && position[4] >= position[2] && position[4] >= position[3] && position[4] >= position[0] && position[4] >= position[5] && position[4] >= position[6])
            {
                bestPosition = 4;
            }

            if (position[5] >= position[1] && position[5] >= position[2] && position[5] >= position[3] && position[5] >= position[4] && position[5] >= position[0] && position[5] >= position[6])
            {
                bestPosition = 5;
            }

            if (position[6] >= position[1] && position[6] >= position[2] && position[6] >= position[3] && position[6] >= position[4] && position[6] >= position[5] && position[6] >= position[0])
            {
                bestPosition = 6;
            }

            this.restetVar();
        }

        //place chip on best position
        Referee.GetComponent<BoardStatus>().changeFieldStatus(bestPosition);

    }

}
