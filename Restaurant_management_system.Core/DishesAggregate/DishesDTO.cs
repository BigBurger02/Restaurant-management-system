﻿using System;
namespace Restaurant_management_system.Core.DishesAggregate
{
	public class DishesDTO
	{
		public int ID { get; set; }
		public string Name { get; set; } = string.Empty;
		public int Price { get; set; }

		public DishesDTO()
		{
		}
	}
}

