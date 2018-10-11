using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Identifier
{
    // The "null" identifier
    zero,

    // Value denoting that the object is a player's hand and should not be destroyed by BinScript
    Hand,

    // Values that can be used to check if a hand is holding something or if it is empty instead
    HandHolding,
    HandEmpty,

    // Value denoting that an object is in the build zone and belongs to a schema in the build zone
    InBuildZone,

    // Value denoting that an object has already been built and does not need to be built again
    Built,

    // Value denoting that an object hasn't yet been loaded into the build zone
    HasNotBeenLoadedInBuildZoneYet,

    // Generic values for objects
    Attachable,
    AttachBase,
    Attached,
    PlayerMoving,
    Dropped,

    // Values denoting specific objects

    // Values for car components
    CarBody,
    CarSeats,
    CarWheels,

    // Values for dishwasher components
    DishBase,
    DishBody1,
    DishBody2,

    // Values for female robot components
    RobotBody,
    RobotEyes,
    RobotHead,
    RobotParts,

    // Values for the test objects used while refining the code
    TestBase,
    TestAttachableA,
    TestAttachableB
}

