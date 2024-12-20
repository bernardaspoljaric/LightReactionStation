using System;
using System.Collections.Generic;

namespace Novena.Helpers {
	public static class CardShuffler {

		public static void ShuffleList<T>(this IList<T> list)
		{
			Random random = new Random();
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = random.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}