using System.Collections.Generic;
using CitizenFX.Core;
using IgiCore.Client.Interface.Map;
using IgiCore.Client.Interface.Map.Loaders;
using static CitizenFX.Core.Native.API;

namespace IgiCore.Client.Managers
{
	public class MapManager : Manager
	{
		public MapRegistry Maps { get; } = new MapRegistry();

		public List<Blip> Blips { get; } = new List<Blip>();

		public readonly List<BlipConfig> BlipConfig = new List<BlipConfig>
		{
			// Mechanics
			new BlipConfig // LSC Burton
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(-337, -135, 39)
			},
			new BlipConfig // LSC by airport
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(-1155, -2007, 13)
			},
			new BlipConfig // LSC La Mesa
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(734, -1085, 22)
			},
			new BlipConfig // LSC Harmony
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(1177, 2640, 37)
			},
			new BlipConfig // LSC Paleto Bay
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(108, 6624, 31)
			},
			new BlipConfig // Mechanic Hawic
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(538, -183, 54)
			},
			new BlipConfig // Mechanic Sandy Shores Airfield
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(1774, 3333, 41)
			},
			new BlipConfig // Mechanic Mirror Park
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(1143, -776, 57)
			},
			new BlipConfig // Mechanic East Joshua Rd.
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(2508, 4103, 38)
			},
			new BlipConfig // Mechanic Sandy Shores gas station
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(2006, 3792, 32)
			},
			new BlipConfig // Hayes Auto, Little Bighorn Ave.
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(484, -1316, 29)
			},
			new BlipConfig // Hayes Auto Body Shop, Del Perro
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(-1419, -450, 36)
			},
			new BlipConfig // Hayes Auto Body Shop, Davis
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(268, -1810, 27)
			},
			//new BlipConfig // Hayes Auto, Rancho (Disabled, looks like a warehouse for the Davis branch)
			//{
			//	Name = "Mechanic",
			//	Sprite = BlipSprite.Repair,
			//	Position = new Vector3(288, -1730, 29)
			//},
			new BlipConfig // Otto's Auto Parts, Sandy Shores
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(1915, 3729, 32)
			},
			new BlipConfig // Mosley Auto Service, Strawberry
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(-29, -1665, 29)
			},
			new BlipConfig // Glass Heroes, Strawberry
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(-212, -1378, 31)
			},
			new BlipConfig // Mechanic Harmony
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(258, 2594, 44)
			},
			new BlipConfig // Simeons
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(-32, -1090, 26)
			},
			new BlipConfig // Bennys
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(-211, -1325, 31)
			},
			new BlipConfig // Auto Repair, Grand Senora Desert
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(903, 3563, 34)
			},
			new BlipConfig // Auto Shop, Grand Senora Desert
			{
				Name = "Mechanic",
				Sprite = BlipSprite.Repair,
				Position = new Vector3(437, 3568, 38)
			},

			// Hospitals
			new BlipConfig // Mount Zonah
			{
				Name = "Hospital",
				Sprite = BlipSprite.Hospital,
				SpriteScale = 0.8f,
				Position = new Vector3(-448, -340, 0)
			},
			new BlipConfig // Pillbox Hill
			{
				Name = "Hospital",
				Sprite = BlipSprite.Hospital,
				SpriteScale = 0.8f,
				Position = new Vector3(375, -596, 0)
			},
			new BlipConfig // Central Los Santos
			{
				Name = "Hospital",
				Sprite = BlipSprite.Hospital,
				SpriteScale = 0.8f,
				Position = new Vector3(340, -1400, 0)
			},
			new BlipConfig // Sandy Shores
			{
				Name = "Hospital",
				Sprite = BlipSprite.Hospital,
				SpriteScale = 0.8f,
				Position = new Vector3(1854, 3700, 0)
			},
			new BlipConfig // Paleto Bay
			{
				Name = "Hospital",
				Sprite = BlipSprite.Hospital,
				SpriteScale = 0.8f,
				Position = new Vector3(-245, 6328, 0)
			},
			//new BlipConfig // St. Fiacre
			//{
			//	Name = "Hospital",
			//	Sprite = BlipSprite.Hospital,
			//  SpriteScale = 0.8f,
			//	Position = new Vector3(1152, -1525, 0)
			//}

			// Police Stations
			new BlipConfig // La Mesa
			{
				Name = "Police Station",
				Sprite = BlipSprite.PoliceStation,
				SpriteScale = 0.8f,
				Position = new Vector3(850.156677246094f, -1283.92004394531f, 28.0047378540039f)
			},
			new BlipConfig // Mission Row
			{
				Name = "Police Station",
				Sprite = BlipSprite.PoliceStation,
				SpriteScale = 0.8f,
				Position = new Vector3(457.956909179688f, -992.72314453125f, 30.6895866394043f)
			},
			new BlipConfig // Sandy Shores
			{
				Name = "Police Station",
				Sprite = BlipSprite.PoliceStation,
				SpriteScale = 0.8f,
				Position = new Vector3(1856.91320800781f, 3689.50073242188f, 34.2670783996582f)
			},
			new BlipConfig // Paleto Bay
			{
				Name = "Police Station",
				Sprite = BlipSprite.PoliceStation,
				SpriteScale = 0.8f,
				Position = new Vector3(-450.063201904297f, 6016.5751953125f, 31.7163734436035f)
			},


			// Airport and Airfield
			new BlipConfig { Name = "Airport", Sprite = BlipSprite.Airport, Position = new Vector3(-1032.690f, -2728.141f, 13.757f) },
			new BlipConfig { Name = "Airport", Sprite = BlipSprite.Airport, Position = new Vector3(1743.6820f, 3286.2510f, 40.087f) },

			// Barbers
			new BlipConfig { Name = "Barber", Sprite = BlipSprite.Barber, Position = new Vector3(-827.333f, -190.916f, 37.599f) },
			new BlipConfig { Name = "Barber", Sprite = BlipSprite.Barber, Position = new Vector3(130.512f, -1715.535f, 29.226f) },
			new BlipConfig { Name = "Barber", Sprite = BlipSprite.Barber, Position = new Vector3(-1291.472f, -1117.230f, 6.641f) },
			new BlipConfig { Name = "Barber", Sprite = BlipSprite.Barber, Position = new Vector3(1936.451f, 3720.533f, 32.638f) },
			new BlipConfig { Name = "Barber", Sprite = BlipSprite.Barber, Position = new Vector3(1200.214f, -468.822f, 66.268f) },
			new BlipConfig { Name = "Barber", Sprite = BlipSprite.Barber, Position = new Vector3(-30.109f, -141.693f, 57.041f) },
			new BlipConfig { Name = "Barber", Sprite = BlipSprite.Barber, Position = new Vector3(-285.238f, 6236.365f, 31.455f) },

			// Stores
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(28.463f, -1353.033f, 29.34f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(-54.937f, -1759.108f, 29.005f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(375.858f, 320.097f, 103.433f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(1143.813f, -980.601f, 46.205f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(1695.284f, 4932.052f, 42.078f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(2686.051f, 3281.089f, 55.241f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(1967.648f, 3735.871f, 32.221f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(-2977.137f, 390.652f, 15.024f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(1160.269f, -333.137f, 68.783f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(-1492.784f, -386.306f, 39.798f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(-1229.355f, -899.230f, 12.263f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(-712.091f, -923.820f, 19.014f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(-1816.544f, 782.072f, 137.6f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(1729.689f, 6405.970f, 34.453f) },
			new BlipConfig { Name = "Store", Sprite = BlipSprite.Store, Position = new Vector3(2565.705f, 385.228f, 108.463f) },

			// Clothing
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(88.291f, -1391.929f, 29.2f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(-718.985f, -158.059f, 36.996f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(-151.204f, -306.837f, 38.724f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(414.646f, -807.452f, 29.338f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(-815.193f, -1083.333f, 11.022f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(-1208.098f, -782.020f, 17.163f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(-1457.954f, -229.426f, 49.185f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(-2.777f, 6518.491f, 31.533f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(1681.586f, 4820.133f, 42.046f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(130.216f, -202.940f, 54.505f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(618.701f, 2740.564f, 41.905f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(1199.169f, 2694.895f, 37.866f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(-3164.172f, 1063.927f, 20.674f) },
			new BlipConfig { Name = "Clothing", Sprite = BlipSprite.Clothes, Position = new Vector3(-1091.373f, 2702.356f, 19.422f) },

			// Ammunation
			new BlipConfig { Name = "Weapon store", Sprite = BlipSprite.AmmuNation, Position = new Vector3(1701.292f, 3750.450f, 34.365f) },
			new BlipConfig { Name = "Weapon store", Sprite = BlipSprite.AmmuNation, Position = new Vector3(237.428f, -43.655f, 69.698f) },
			new BlipConfig { Name = "Weapon store", Sprite = BlipSprite.AmmuNation, Position = new Vector3(843.604f, -1017.784f, 27.546f) },
			new BlipConfig { Name = "Weapon store", Sprite = BlipSprite.AmmuNation, Position = new Vector3(-321.524f, 6072.479f, 31.299f) },
			new BlipConfig { Name = "Weapon store", Sprite = BlipSprite.AmmuNation, Position = new Vector3(-664.218f, -950.097f, 21.509f) },
			new BlipConfig { Name = "Weapon store", Sprite = BlipSprite.AmmuNation, Position = new Vector3(-1320.983f, -389.260f, 36.483f) },
			new BlipConfig { Name = "Weapon store", Sprite = BlipSprite.AmmuNation, Position = new Vector3(-1109.053f, 2686.300f, 18.775f) },
			new BlipConfig { Name = "Weapon store", Sprite = BlipSprite.AmmuNation, Position = new Vector3(2568.379f, 309.629f, 108.461f) },
			new BlipConfig { Name = "Weapon store", Sprite = BlipSprite.AmmuNation, Position = new Vector3(-3157.450f, 1079.633f, 20.692f) },

			//// Basic
			//new BlipConfig { Name = "Comedy Club", id=102, Position = new Vector3(377.088f, -991.869f, -97.604) },
			//new BlipConfig { Name = "Franklin", id=210, Position = new Vector3(7.900, 548.100, 175.5) },
			//new BlipConfig { Name = "Franklin", id=210, Position = new Vector3(-14.128, y=-1445.483,    z=30.648) },
			//new BlipConfig { Name = "Michael", id=124, Position = new Vector3(-852.400, 160f, 65.6) },
			//new BlipConfig { Name = "Trevor", id=208, Position = new Vector3(1985.700, 3812.200, 32.2) },
			//new BlipConfig { Name = "Trevor", id=208, Position = new Vector3(-1159.034, y=-1521.180, 10.633) },
			//new BlipConfig { Name = "FIB", id=106, Position = new Vector3(105.455f, -745.483, 44.754) },
			//new BlipConfig { Name = "Lifeinvader", Sprite = BlipSprite.Lester, Position = new Vector3(-1047.900f, -233f, 39f) },
			//new BlipConfig { Name = "Cluckin Bell", Sprite = BlipSprite.Garage, Position = new Vector3(-72.68752, 6253.72656, 31.08991) },
			//new BlipConfig { Name = "Tequil-La La", id=93, Position = new Vector3(-565.171, 276.625, 83.286) },
			//new BlipConfig { Name = "O'Neil Ranch", id=438, Position = new Vector3(2441.200, 4968.500, 51.7) },
			//new BlipConfig { Name = "Play Boy Mansion", id=439, Position = new Vector3(-1475.234, 167.088, 55.841) },
			//new BlipConfig { Name = "Hippy Camp", id=140, Position = new Vector3(2476.712, 3789.645, 41.226) },
			//new BlipConfig { Name = "Chop shop", id=446, Position = new Vector3(479.056f, -1316.825, 28.203) },
			//new BlipConfig { Name = "Rebel Radio", id=136, Position = new Vector3(736.153, 2583.143, 79.634) },
			//new BlipConfig { Name = "Morgue", id=310, Position = new Vector3(243.351f, -1376.014, 39.534) },
			//new BlipConfig { Name = "Golf", id=109, Position = new Vector3(-1336.715, 59.051, 55.246 ) },
			//new BlipConfig { Name = "Jewelry Store", Sprite = BlipSprite.Store, x=-630.400f, -236.700, 40.) },

			//// Property
			//new BlipConfig { Name = "Casino", id=207, Position = new Vector3(925.329f, 46.152f, 80.908f) },
			//new BlipConfig { Name = "Maze Bank Arena", id=135, Position = new Vector3(-250.604f, -2030f, 30f) },
			//new BlipConfig { Name = "Stripbar", id=121, Position = new Vector3(134.476f, -1307.887f, 28.983f) },
			//new BlipConfig { Name = "Smoke on the Water", id=140, Position = new Vector3(-1171.42f, -1572.72f, 3.6636f) },
			//new BlipConfig { Name = "Weed Farm", id=140, Position = new Vector3(2208.777f, 5578.235f, 53.735f) },
			//new BlipConfig { Name = "Downtown Cab Co", id=375, Position = new Vector3(900.461f, -181.466f, 73.89f) },
			//new BlipConfig { Name = "Theater", id=135, Position = new Vector3(293.089f, 180.466f, 104.301f) },

			//// Gangs
			//new BlipConfig { Name = "Gang", id=437, Position = new Vector3(298.68f, -2010.10f, 20.07f) },
			//new BlipConfig { Name = "Gang", id=437, Position = new Vector3(86.64f, -1924.60f, 20.79f) },
			//new BlipConfig { Name = "Gang", id=437, Position = new Vector3(-183.52f, -1632.62f, 33.34f) },
			//new BlipConfig { Name = "Gang", id=437, Position = new Vector3(989.37f, -1777.56f, 31.32f) },
			//new BlipConfig { Name = "Gang", id=437, Position = new Vector3(960.24f, -140.31f, 74.5f) },
			//new BlipConfig { Name = "Gang", id=437, Position = new Vector3(-1042.29f, 4910.17f, 94.92f) },

			// Gas stations
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(49.4187f, 2778.793f, 58.043f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(263.894f, 2606.463f, 44.983f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(1039.958f, 2671.134f, 39.55f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(1207.260f, 2660.175f, 37.899f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(2539.685f, 2594.192f, 37.944f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(2679.858f, 3263.946f, 55.24f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(2005.055f, 3773.887f, 32.403f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(1687.156f, 4929.392f, 42.078f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(1701.314f, 6416.028f, 32.763f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(179.857f, 6602.839f, 31.868f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(-94.4619f, 6419.594f, 31.489f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(-2554.996f, 2334.40f, 33.078f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(-1800.375f, 803.661f, 138.651f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(-1437.622f, -276.747f, 46.207f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(-2096.243f, -320.286f, 13.168f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(-724.619f, -935.1631f, 19.213f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(-526.019f, -1211.003f, 18.184f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(-70.2148f, -1761.792f, 29.534f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(265.648f, -1261.309f, 29.292f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(819.653f, -1028.846f, 26.403f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(1208.951f, -1402.567f, 35.224f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(1181.381f, -330.847f, 69.316f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(620.843f, 269.100f, 103.089f) },
			new BlipConfig { Name = "Gas Station", Sprite = BlipSprite.JerryCan, Position = new Vector3(2581.321f, 362.039f, 108.468f) },

			// Police Stations
			new BlipConfig { Name = "Police Station", Sprite = BlipSprite.PoliceStation, Position = new Vector3(425.130f, -979.558f, 30.711f) },
			new BlipConfig { Name = "Police Station", Sprite = BlipSprite.PoliceStation, Position = new Vector3(1859.234f, 3678.742f, 33.69f) },
			new BlipConfig { Name = "Police Station", Sprite = BlipSprite.PoliceStation, Position = new Vector3(-438.862f, 6020.768f, 31.49f) },
			new BlipConfig { Name = "Police Station", Sprite = BlipSprite.PoliceStation, Position = new Vector3(818.221f, -1289.883f, 26.3f) },

			//new BlipConfig { Name = "Prison", id=285, Position = new Vector3(1679.049f, 2513.711f, 45.565f) },

			// Hospitals
			new BlipConfig { Name = "Hospital", Sprite = BlipSprite.Hospital, Position = new Vector3(1839.6f, 3672.93f, 34.28f) },
			new BlipConfig { Name = "Hospital", Sprite = BlipSprite.Hospital, Position = new Vector3(-247.76f, 6331.23f, 32.43f) },
			new BlipConfig { Name = "Hospital", Sprite = BlipSprite.Hospital, Position = new Vector3(-449.67f, -340.83f, 34.5f) },
			new BlipConfig { Name = "Hospital", Sprite = BlipSprite.Hospital, Position = new Vector3(357.43f, -593.36f, 28.79f) },
			new BlipConfig { Name = "Hospital", Sprite = BlipSprite.Hospital, Position = new Vector3(295.83f, -1446.94f, 29.97f) },
			new BlipConfig { Name = "Hospital", Sprite = BlipSprite.Hospital, Position = new Vector3(-676.98f, 310.68f, 83.08f) },
			new BlipConfig { Name = "Hospital", Sprite = BlipSprite.Hospital, Position = new Vector3(1151.21f, -1529.62f, 35.37f) },
			new BlipConfig { Name = "Hospital", Sprite = BlipSprite.Hospital, Position = new Vector3(-874.64f, -307.71f, 39.58f) },

			// Vehicle Shop (Simeon)
			//new BlipConfig { Name = "Simeon", id=120, Position = new Vector3(-33.803f, -1102.322f, 25.422f) },

			// LS Customs
			new BlipConfig { Name = "LS Customs", Sprite = BlipSprite.LosSantosCustoms, Position = new Vector3(-362.796f, -132.400f, 38.252f) },
			new BlipConfig { Name = "LS Customs", Sprite = BlipSprite.LosSantosCustoms, Position = new Vector3(-1140.19f, -1985.478f, 12.729f) },
			new BlipConfig { Name = "LS Customs", Sprite = BlipSprite.LosSantosCustoms, Position = new Vector3(716.464f, -1088.869f, 21.929f) },
			new BlipConfig { Name = "LS Customs", Sprite = BlipSprite.LosSantosCustoms, Position = new Vector3(1174.81f, 2649.954f, 37.371f) },
			new BlipConfig { Name = "LS Customs", Sprite = BlipSprite.LosSantosCustoms, Position = new Vector3(118.485f, 6619.560f, 31.802f) },

			// Lester
			new BlipConfig { Name = "Lester", Sprite = BlipSprite.Lester, Position = new Vector3(1248.183f, -1728.104f, 56f) },
			new BlipConfig { Name = "Lester", Sprite = BlipSprite.Lester, Position = new Vector3(719f, -975f, 25f) },

			// Survivals
			new BlipConfig { Name = "Survival", Sprite = BlipSprite.GTAOSurvival, Position = new Vector3(2351.331f, 3086.969f, 48.057f) },
			new BlipConfig { Name = "Survival", Sprite = BlipSprite.GTAOSurvival, Position = new Vector3(-1695.803f, -1139.190f, 13.152f) },
			new BlipConfig { Name = "Survival", Sprite = BlipSprite.GTAOSurvival, Position = new Vector3(1532.52f, -2138.682f, 77.12f) },
			new BlipConfig { Name = "Survival", Sprite = BlipSprite.GTAOSurvival, Position = new Vector3(-593.724f, 5283.231f, 70.23f) },
			new BlipConfig { Name = "Survival", Sprite = BlipSprite.GTAOSurvival, Position = new Vector3(1891.436f, 3737.409f, 32.513f) },
			new BlipConfig { Name = "Survival", Sprite = BlipSprite.GTAOSurvival, Position = new Vector3(195.572f, -942.493f, 30.692f) },
			new BlipConfig { Name = "Survival", Sprite = BlipSprite.GTAOSurvival, Position = new Vector3(1488.579f, 3582.804f, 35.345f) },

			new BlipConfig { Name = "Safehouse", Sprite = BlipSprite.Garage, Position = new Vector3(-952.35943603516f, -1077.5021972656f, 2.6772258281708f) },
			new BlipConfig { Name = "Safehouse", Sprite = BlipSprite.Garage, Position = new Vector3(-59.124889373779f, -616.55456542969f, 37.356777191162f) },
			new BlipConfig { Name = "Safehouse", Sprite = BlipSprite.Garage, Position = new Vector3(-255.05390930176f, -943.32885742188f, 31.219989776611f) },
			new BlipConfig { Name = "Safehouse", Sprite = BlipSprite.Garage, Position = new Vector3(-771.79888916016f, 351.59423828125f, 87.998191833496f) },
			new BlipConfig { Name = "Safehouse", Sprite = BlipSprite.Garage, Position = new Vector3(-3086.428f, 339.252f, 6.371f) },
			new BlipConfig { Name = "Safehouse", Sprite = BlipSprite.Garage, Position = new Vector3(-917.289f, -450.206f, 39.6f) },

			new BlipConfig { Name = "Race", Sprite = BlipSprite.RaceSea, Position = new Vector3(-1277.629f, -2030.913f, 1.2823f) },
			new BlipConfig { Name = "Race", Sprite = BlipSprite.RaceSea, Position = new Vector3(2384.969f, 4277.583f, 30.379f) },
			new BlipConfig { Name = "Race", Sprite = BlipSprite.RaceSea, Position = new Vector3(1577.881f, 3836.107f, 30.7717f) },

			// Yacht
			new BlipConfig { Name = "Yacht", Sprite = BlipSprite.Boat, Position = new Vector3(-2045.800f, -1031.200f, 11.9f) },
			new BlipConfig { Name = "Cargoship", Sprite = BlipSprite.Boat, Position = new Vector3(-90f, -2365.800f, 14.3f) },

			// Bahama Mamas West
			new BlipConfig { Name = "Bahama Mamas West", Sprite = BlipSprite.Bar, Position = new Vector3(-1387.975f, -587.7377f, 30.21593f) },
		};

		public MapManager()
		{
			LoadMpDlcMaps(); // TODO: Needed?
			EnableMpDlcMaps(true); // TODO: Needed?

			this.Maps = new MapRegistry
			{
				new BahamaMamasWest(),
				new GrapeseedFarm(),
				new TrevorsTrailer(),
			};

			this.Maps.Load();

			foreach (var blipConfig in this.BlipConfig)
			{
				var blip = World.CreateBlip(blipConfig.Position);
				blip.Sprite = blipConfig.Sprite;
				blip.Scale = blipConfig.SpriteScale;
				blip.Name = blipConfig.Name;
				blip.IsShortRange = !blipConfig.PinMinimap;
				blip.Color = blipConfig.Color;

				this.Blips.Add(blip);
			}
		}

		public override void Dispose()
		{
			foreach (Blip blip in this.Blips)
			{
				blip.Delete();
			}
		}
	}
}
