using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Database
{
   public static List<Wall> walls = new List<Wall>(){
       new Wall() {cost=10},
       new Wall() {cost=30},
       new Wall() {cost=80},
       new Wall() {cost=170},
   };

   public static List<Weapon> weapons = new List<Weapon>(){
       new Weapon(){
           range = 5,
           damage = 10,
           fireSpeed = 2,
           armorIgnore = 0,
           shieldIgnore = 0,
           cost = 15
       },
       new Weapon(){
           range = 15,
           damage = 15,
           fireSpeed = 8,
           armorIgnore = 0,
           shieldIgnore = 0,
           cost = 30
       },
       new Weapon(){
           range = 3,
           damage = 5,
           fireSpeed = 1,
           armorIgnore = 0,
           shieldIgnore = 0,
           cost = 20
       },
   };
}