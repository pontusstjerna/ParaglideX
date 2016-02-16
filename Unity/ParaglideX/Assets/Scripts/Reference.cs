using System.Collections;

public static class Reference {
	public const float MOUSE_SENSITIVITY = 3f;
	public const float PLAYER_WEIGHT = 80f;
	public const float GRAVITY = 9.807f;
	public const float LOOK_UP_LIMIT = 270f;
	public const float LOOK_DOWN_LIMIT = 90f;
	public const float FLYABLE_ANGLE = 30f;
	public const float PLAYER_MASS = PLAYER_WEIGHT*10;
	public const float DRAG_COEFFICIENT_FRONT = 0.42f; //https://en.wikipedia.org/wiki/Drag_coefficient
	public const float DRAG_COEFFICIENT_UNDER = 0.6f;
	public const float AIR_DENSITY_20 = 1.2041f; //https://en.wikipedia.org/wiki/Density_of_air
	public const float AREA_UNDER = 22;
	public const float AREA_FRONT = 3;
}
