using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RaspberryEngine.Particles {
	public class EmitterSettings {
		//Particle Settings these are presets
		public string Name;
		public float SpawnRate;
		public float MinVelocity;
		public float MaxVelocity;
		public float MinVelocityAngle;
		public float MaxVelocityAngle;
		public float MinAngle;
		public float MaxAngle;
		public float MinAngleVelocity;
		public float MaxAngleVelocity;
		public Color Color1;
		public Color Color2;
		public Color Color3;
		public float MinScale;
		public float MaxScale;
		public float MinScaleVelocity;
		public float MaxScaleVelocity;
		public float MinLifeVelocity;
		public float MaxLifeVelocity;

		public EmitterSettings(string name, float SpawnRate,
		float minVelocity, float maxVelocity, float minVelocityAngle, float maxVelocityAngle,
		float minAngle, float maxAngle, float minAngleVelocity, float maxAngleVelocity,
		float minScale, float maxScale, float minScaleVelocity, float maxScaleVelocity,
		float minLifeVelocity, float maxLifeVelocity,
		Color color1, Color color2, Color color3) {
			Name = name;
			this.SpawnRate = SpawnRate;
			MinVelocity = minVelocity;
			MaxVelocity = maxVelocity;
			MinVelocityAngle = minVelocityAngle;
			MaxVelocityAngle = maxVelocityAngle;
			MinAngle = minAngle;
			MaxAngle = maxAngle;
			MinAngleVelocity = minAngleVelocity;
			MaxAngleVelocity = maxAngleVelocity;
			Color1 = color1;
			Color2 = color2;
			Color3 = color3;
			MinScale = minScale;
			MaxScale = maxScale;
			MinScaleVelocity = minScaleVelocity;
			MaxScaleVelocity = maxScaleVelocity;
			MinLifeVelocity = minLifeVelocity;
			MaxLifeVelocity = maxLifeVelocity;
		}

		#region Preset
		public static EmitterSettings[] PresetSettings =
            {
                new EmitterSettings(
                "Sparkler", // Name (its not used. only exist for you to know what type of paricle this is)
                1f, //the rate of wich particles spawnevery frame(1 = one every frame, 0.1f = 10 every frame, 5 = spawn one every 5th frame)
                0f,5f, // movement 'Speed'
                0,360, // movement 'Direction'
                0,360,  // Start 'Angle'
                0,0,    // rotation 'Speed'
                0.01f,0.4f, // Start 'Scale'
                0f,0.01f, // Scale 'Speed'
                0.05f,0.5f,  // Life 'Drain'
                Color.White, Color.Orange, Color.OrangeRed), // 'Colors' to shift between

                new EmitterSettings(
                "Glow", // Name (its not used. only exist for you to know what type of paricle this is)
                0.1f, //the rate of wich particles spawnevery frame(1 = one every frame, 0.1f = 10 every frame, 5 = spawn one every 5th frame)
                0f,3f, // movement 'Speed'
                0,360, // movement 'Direction'
                0,360,  // Start 'Angle'
                0,1,    // rotation 'Speed'
                0f,0.5f, // Start 'Scale'
                0.002f,0.005f, // Scale 'Speed'
                0.05f,0.05f,  // Life 'Drain'
                Color.Transparent, Color.White, Color.Cyan), // 'Colors' to shift between

                new EmitterSettings(
                "Fire", // Name (its not used. only exist for you to know what type of paricle this is)
                1f, //the rate of wich particles spawnevery frame(1 = one every frame, 0.1f = 10 every frame, 5 = spawn one every 5th frame)
                0f,4f, // movement 'Speed'
                160,200, // movement 'Direction'
                0,360,  // Start 'Angle'
                -0.1f,0.1f,    // rotation 'Speed'
                0.2f,0.3f, // Start 'Scale'
                -0.001f,-0.01f, // Scale 'Speed'
                0.05f,0.1f,  // Life 'Drain'
                Color.LightYellow, Color.DarkOrange, Color.Red), // 'Colors' to shift between

                new EmitterSettings(
                "Smoke", // Name (its not used. only exist for you to know what type of paricle this is)
                5f, //the rate of wich particles spawnevery frame(1 = one every frame, 0.1f = 10 every frame, 5 = spawn one every 5th frame)
                1f,1f, // movement 'Speed'
                180,180, // movement 'Direction'
                0,360,  // Start 'Angle'
                -0.01f,0.01f,    // rotation 'Speed'
                0.2f,0.2f, // Start 'Scale'
                0.0001f,0.001f, // Scale 'Speed'
                0.005f,0.005f,  // Life 'Drain'
                Color.Transparent, Color.Gray, Color.Black), // 'Colors' to shift between
        };
		#endregion
	}
}
