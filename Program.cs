using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace graphWaterProblem
{
    class Program
    {
        enum tanks {cuatro = 4, siete = 7, diez = 10};
        static List<int> verificados = new List<int>();

        static void printMatrix(int[,] matrix) 
        {
            Console.WriteLine("--------------------------");
            for (int i = 0;i<3;i++)
            {
                for(int l = 0; l < 11; l++)
                {
                    Console.Write(matrix[i, l] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("--------------------------");
        }

        static int make_code(int i, int l , int posReceptor, int w)
        {
            int codigo = 0;
            // se respeta un orden
            // char 1 representa capacidad del tanque de 10 litros
            // char 2 representa capacidad del tanque de 7 litros
            // char 3 representa capacidad del tanque de 4 litros

            //tanque de 10 debe ser el char si o si por que nuestra configuracion matricial acepta
            //10 litros lo que implica tener numeros como 10-3-3 complicados de representar 
            //como 3-3-10 o 3-10-3

            //desconocemos siempre la cantidad de un estado pero sabemos que la suma de todos
            //los tanques es 11 --> l + w == 11
            int tanqRestante = 11 -l-w;
            if (i == 2) 
            {
                codigo += l;
                codigo *= 100;
                if(posReceptor == 1)
                {
                    codigo += (w*10);
                    codigo += tanqRestante;
                }
                else
                {
                    codigo += (tanqRestante*10);
                    codigo += w;
                }
            }
            else if(posReceptor == 2)
            {
                codigo += w;
                codigo *= 100;
                if (i == 1)
                {
                    codigo += (l * 10);
                    codigo += tanqRestante;
                }
                else
                {
                    codigo += (tanqRestante * 10);
                    codigo += l;
                }
            }
            else
            {
                codigo += tanqRestante;
                codigo *= 100;
                if (i == 1)
                {
                    codigo += (l * 10);
                    codigo += w;
                }
                else
                {
                    codigo += (w * 10);
                    codigo += l;
                }
            }

            //Console.WriteLine(i+" "+l+" "+posReceptor+" "+w+" "+tanqRestante);
            Console.WriteLine(codigo);
            return codigo;
        }
        static bool recursion(int[,] matriz)
        {
            printMatrix(matriz);
            if (matriz[0, 2] == 1 && matriz[1, 2] == 1){
                return true;
            }
            
            //hago todas las posibilidades
            for(int i = 0; i < 3; i++)
            {
                for(int l = 1; l < 11; l++)
                {
                    if (matriz[i, l] == 1)
                    {
                        //elijo pasar a cualquiera de los dos tanques
                        //actualizo mi capacidad
                        //llamo a la recursion
                        //si retorna falso vuelvo a mi estado anteior
                        tanks opcion1, opcion2;
   
                        //opcion 1
                        if (i == 0)
                        {
                            opcion1 = tanks.siete;
                            opcion2 = tanks.diez;
                        }
                        else if (i == 1) 
                        {
                            opcion1 = tanks.cuatro;
                            opcion2 = tanks.diez;
                        }
                        else 
                        {
                            opcion1 = tanks.cuatro;
                            opcion2 = tanks.siete;
                        }

                        //necesito la capcidad actual de opcion1
                        int w;
                        int posReceptor = (int)opcion1 == 4 ? 0 : ((int)opcion1 == 7 ? 1 : 2);
                        for (w = 0; w < 11; w++)
                        {
                            if (matriz[posReceptor, w] == 1) break;
                        }

                        //si donde estoy ya fue verificado NO LO VERIFICO!
                        int codigo = make_code(i, l, posReceptor, w);

                        //w es la capacidad actual del receptor
                        //capacidad maxima (int)opcion1 del receptor
                        //si w == opcion1 
                        //no peudo agregar mas (el tanque ya esta lleno)

                        if (!verificados.Contains(codigo))
                        {
                            verificados.Add(codigo); //agrego a los estados que ya se verficaron
                            if (w < (int)opcion1)
                            {
                                verificados.Add(codigo); //agrego a los estados que ya se verficaron
                                int q = (int)opcion1 - w; //cantidad que puede recibir el receptor  capcidad maxima menos lo que tiene
                                matriz[i, l] = 0;//deja de ser lo que realmente tiene el emisor
                                if (q > l) //si la cantidad q puede recibir el receptor es mayor q la disponible
                                {
                                    matriz[i, 0] = 1;
                                    q = l; //puede recibir como maximo l
                                }
                                else  //el emisor envia una cantidad q y su estado actual es lo que tenia menos ese q
                                {
                                    matriz[i, l - q] = 1;
                                }

                                //ahora modifico el receptor
                                matriz[posReceptor, w] = 0; //posicion antigua
                                matriz[posReceptor, w + q] = 1;//posicion nueva


                                if (recursion(matriz)) return true;
                                else
                                {
                                    //vuelvo al estado inicial
                                    matriz[i, l] = 1;
                                    matriz[i, l - q] = 0;

                                    matriz[posReceptor, w] = 1;
                                    matriz[posReceptor, w + q] = 0;
                                }
                            }
                        }

                        //opcion 2 mismo que opcion 1
                        //necesito la capcidad actual de opcion2

                        posReceptor = (int)opcion2 == 4 ? 0 : ((int)opcion2 == 7 ? 1 : 2);
                        for (w = 0; w < 11; w++)
                        {
                            if (matriz[posReceptor, w] == 1) break;
                        }

                        //si donde estoy ya fue verificado NO LO VERIFICO!
                        codigo = make_code(i, l, posReceptor, w);


                        //w es la capacidad actual del receptor
                        //capacidad maxima (int)opcion2 del receptor
                        //si w == opcion2 
                        //no peudo agregar mas (el tanque ya esta lleno)

                        if (!verificados.Contains(codigo))
                        {
                            verificados.Add(codigo); //agrego a los estados que ya se verficaron
                            if (w < (int)opcion2)
                            {
                                int q = (int)opcion2 - w; //cantidad que puede recibir el receptor
                                matriz[i, l] = 0;//deja de ser lo que realmente tiene el emisor
                                if (q > l) //si la cantidad q puede recibir el receptor es mayor q la disponible
                                {
                                    matriz[i, 0] = 1;
                                    q = l; //puede recibir como maximo l
                                }
                                else  //el emisor envia una cantidad q y su estado actual es lo que tenia menos ese q
                                {
                                    matriz[i, l - q] = 1;
                                }

                                //ahora modifico el receptor
                                matriz[posReceptor, w] = 0; //posicion antigua
                                matriz[posReceptor, w + q] = 1;//posicion nueva


                                if (recursion(matriz)) return true;
                                else
                                {
                                    //vuelvo al estado inicial
                                    matriz[i, l] = 1;
                                    matriz[i, l - q] = 0;

                                    matriz[posReceptor, w] = 1;
                                    matriz[posReceptor, w + q] = 0;
                                }
                            }
                        }


                    }
                }
            }
            return false;
        }

        static void Main(string[] args)
        {
            int[,] matriz = new int[,] {
                { 0, 0, 0 , 0 , 1, 0, 0, 0, 0, 0, 0},//4
                { 0, 0, 0 , 0 , 0, 0, 0, 1, 0, 0, 0},//7
                { 1, 0, 0 , 0 , 0, 0, 0, 0, 0, 0, 0}};//10
            Console.WriteLine(recursion(matriz));
        }
    }
}
