using System;
using System.Collections.Generic;
using System.Linq;

namespace VarmDrinkStation
{
    // Definierar ett interface för varma drycker
    public interface IWarmDrink
    {
        void Consume(); // Metod för att konsumera drycken
    }

    // Implementerar en specifik varm dryck, i detta fall vatten
    internal class Water : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Warm water is served."); // Utskrift vid konsumtion av vatten
        }
    }

    //Coffee
    internal class Coffee : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Coffee is served."); // Utskrift vid konsumtion av kaffe
        }
    }

    //Cappuccino
    internal class Cappuccino : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Cappuccino is served."); // Utskrift vid konsumtion av cappuccino
        }
    }

    //Hot cocoa
    internal class Cocoa : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Hot cocoa is served."); // Utskrift vid konsumtion av varm choklad
        }
    }

    // Definierar ett interface för fabriker som kan skapa varma drycker
    public interface IWarmDrinkFactory
    {
        IWarmDrink Prepare(int total); // Metod för att förbereda drycken med en specifik mängd
    }

    // Implementerar en specifik fabrik som förbereder varmt vatten
    internal class HotWaterFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} cups of hot water into your mug."); // Utskrift av mängden vatten som hälls upp
            return new Water(); // Returnerar en ny instans av Water
        }
    }

    internal class CoffeeFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} cups of hot water over a coffee filter that contains {total * 2} teaspoons of grinded coffee beans. Let it drip into the mug. Remove the coffee filter.");
            return new Coffee(); // Returnerar en ny instans av Coffee
        }
    }

    internal class CappuccinoFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Brew {total} shot/s of espresso in a cappuccino cup. Steam {total} cup/s of milk and add 1/3 of the steamed milk and a 1/3 of the creamy foam.");
            return new Cappuccino(); // Returnerar en ny instans av Cappuccino
        }
    }

    internal class CocoaFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} cups of hot milk into your mug and mix it with {total / 2} tea spoon(s) pre-made cocoa mix (50/50 sugar and cocoa).");
            return new Cocoa(); // Returnerar en ny instans av Cocoa
        }
    }

    // Maskin som hanterar skapandet av varma drycker
    public class WarmDrinkMachine
    {
        private readonly List<Tuple<string, IWarmDrinkFactory>> namedFactories; // Lista över fabriker med deras namn

        public WarmDrinkMachine()
        {
            namedFactories = new List<Tuple<string, IWarmDrinkFactory>>(); // Initierar listan över fabriker

            // Registrerar fabriker explicit
            RegisterFactory<HotWaterFactory>("Hot Water"); // Registrerar fabriken för varmt vatten
            RegisterFactory<CoffeeFactory>("Coffee");
            RegisterFactory<CappuccinoFactory>("Cappuccino");
            RegisterFactory<CocoaFactory>("Hot Cocoa");
        }

        // Metod för att registrera en fabrik
        private void RegisterFactory<T>(string drinkName) where T : IWarmDrinkFactory, new()
        {
            namedFactories.Add(Tuple.Create(drinkName, (IWarmDrinkFactory)Activator.CreateInstance(typeof(T)))); // Lägger till fabriken i listan
        }

        // Metod för att skapa en varm dryck
        public IWarmDrink MakeDrink()
        {
            Console.WriteLine("This is what we serve today:");
            for (var index = 0; index < namedFactories.Count; index++)
            {
                var tuple = namedFactories[index];
                Console.WriteLine($"{index}: {tuple.Item1}"); // Skriver ut tillgängliga drycker
            }
            Console.WriteLine("Select a number to continue:");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int i) && i >= 0 && i < namedFactories.Count) // Läser och validerar användarens val
                {
                    Console.Write("How many cups: ");
                    if (int.TryParse(Console.ReadLine(), out int total) && total > 0) // Läser och validerar mängden
                    {
                        return namedFactories[i].Item2.Prepare(total); // Förbereder och returnerar drycken
                    }
                }
                Console.WriteLine("Something went wrong with your input, try again."); // Meddelande vid felaktig inmatning
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var machine = new WarmDrinkMachine(); // Skapar en instans av WarmDrinkMachine
            while (true)
            {
                IWarmDrink drink = machine.MakeDrink(); // Skapar en dryck
                drink.Consume(); // Konsumerar drycken
                Console.WriteLine("Do you want to make another drink? (yes/no):");
                string input = Console.ReadLine().ToLower();
                if (input != "yes")
                {
                    break; 
                }
            }
        }
    }
}