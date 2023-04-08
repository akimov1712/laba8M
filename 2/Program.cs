using System;
using System.Diagnostics;

class Program
{
	static void Main(string[] args)
	{
		string text = "Следует отметить, что высокое качество позиционных исследований, а также свежий взгляд на привычные вещи — безусловно открывает новые горизонты для прогресса профессионального сообщества. Ясность нашей позиции очевидна: выбранный нами инновационный путь выявляет срочную потребность поставленных обществом задач! А ещё предприниматели в сети интернет разоблачены. Лишь сторонники тоталитаризма в науке обнародованы. Задача организации, в особенности же высококачественный прототип будущего проекта в значительной степени обусловливает важность соответствующих условий активизации. В своём стремлении повысить качество жизни, они забывают, что сложившаяся структура организации, а также свежий взгляд на привычные вещи — безусловно открывает новые горизонты для поставленных обществом задач. Мы вынуждены отталкиваться от того, что перспективное планирование обеспечивает актуальность модели развития.";
		string[] pattern = {
			"свежий взгляд",
			"Задача",
			"задач! А ещё",
			"поставленных обществом",
			"вещи — безусловно",
			"обеспечивает актуальность модели развития."
		};

		for(int i = 0; i < pattern.Length; i++) {
			Console.WriteLine($"Поиск подстроки '{pattern[i]}'");
			KMP(text, pattern[i]);
			BM(text, pattern[i]);
			Console.WriteLine();
		}

	}

	static int[] PrefixFunction(string pattern)
	{
		int[] pi = new int[pattern.Length];
		pi[0] = 0;
		int j = 0;
		for (int i = 1; i < pattern.Length; i++)
		{
			while (j > 0 && pattern[j] != pattern[i])
			{
				j = pi[j - 1];
			}
			if (pattern[j] == pattern[i])
			{
				j++;
			}
			pi[i] = j;
		}
		return pi;
	}

	static void KMP(string text, string pattern)
	{
		int[] pi = PrefixFunction(pattern);
		int j = 0;
		int comparisons = 0;
		Stopwatch sw = new Stopwatch();
		sw.Start();
		for (int i = 0; i < text.Length; i++)
		{
			comparisons++;
			while (j > 0 && pattern[j] != text[i])
			{
				j = pi[j - 1];
				comparisons++;
			}
			if (pattern[j] == text[i])
			{
				j++;
				comparisons++;
			}
			if (j == pattern.Length)
			{
				sw.Stop();
				Console.WriteLine($"Алгоритм Кнута-Морриса-Пратта: Подстрока найдена на позиции {i - j + 1}. Сравнений: {comparisons}. Время выполнения: {sw.Elapsed.Seconds}:{sw.ElapsedMilliseconds} ({sw.Elapsed})");
				return;
			}
		}
		sw.Stop();
		Console.WriteLine($"Алгоритм Кнута-Морриса-Пратта: Не найдено. Сравнений: {comparisons}. Время выполнения: {sw.Elapsed.Seconds}:{sw.ElapsedMilliseconds} ({sw.Elapsed})");
	}

	static void BM(string text, string pattern)
	{
		int n = text.Length;
		int m = pattern.Length;
		// Создание таблицы суффиксов и хеш-таблицы для быстрого поиска смещений
		int[] suffix = new int[m];
		Dictionary<char, int> shifts = new Dictionary<char, int>();

		int lastPrefixPosition = m;
		for (int i = m - 1; i >= 0; i--)
		{
			if (IsPrefix(pattern, i + 1))
			{
				lastPrefixPosition = i + 1;
			}
			suffix[m - 1 - i] = lastPrefixPosition - i + m - 1;
		}

		for (int i = 0; i < m - 1; i++)
		{
			char ch = pattern[i];
			if (!shifts.ContainsKey(ch))
			{
				shifts.Add(ch, m - 1 - i);
			}
		}

		// Поиск с использованием таблицы суффиксов и хеш-таблицы
		int j = 0;
		int comparisons = 0;
		Stopwatch sw = new Stopwatch(); 
		sw.Start();
		while (j <= n - m)
		{
			int i = m - 1;
			while (i >= 0 && pattern[i] == text[i + j])
			{
				i--;
				comparisons++;
			}
			if (i < 0)
			{
				sw.Stop();
				Console.WriteLine($"Алгоритм Бойера-Мура: Подстрока найдена на позиции {j}. Сравнений: {comparisons}. Время выполнения: {sw.Elapsed.Seconds}:{sw.ElapsedMilliseconds} ({sw.Elapsed})");
				return;
			}
			else
			{
				comparisons++;
				char ch = text[i + j];
				if (shifts.ContainsKey(ch))
				{
					j += shifts[ch];
				}
				else
				{
					j += m;
				}
			}
		}
		sw.Stop();
		Console.WriteLine($"Алгоритм Бойера-Мура: Не найдено. Сравнений: {comparisons}. Время выполнения: {sw.Elapsed.Seconds}:{sw.ElapsedMilliseconds} ({sw.Elapsed})");
	}
	static bool IsPrefix(string pattern, int p)
	{
		int j = 0;
		for (int i = p; i < pattern.Length; i++)
		{
			if (pattern[i] != pattern[j])
			{
				return false;
			}
			j++;
		}
		return true;
	}
}