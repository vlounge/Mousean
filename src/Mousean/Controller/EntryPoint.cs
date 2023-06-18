namespace Mousean.Controller;

public class EntryPoint {
  public static MouseanGame Game;
  static void Main()
  {
      using (Game = new MouseanGame())
        Game.Run();
  }
}