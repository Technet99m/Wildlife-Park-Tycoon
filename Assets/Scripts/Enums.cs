﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Food
{
    water = 0,
    meat,
    grass,
    fish,
    veggie,
    fruit
}
public enum ItemType
{
    feeder,
    special,
    decor
}
public enum Special
{
    jump = 0,
    run,
    swim,
	sleep,
    mud,
    scratch
}
public enum NeedType
{
    Food,
    Sex,
    Special
}