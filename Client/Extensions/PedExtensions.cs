using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using IgiCore.Core.Extensions;

namespace IgiCore.Client.Extensions
{
    public static class PedExtensions
    {
        public static async Task RunTaskSequence(this Ped ped, TaskSequence sequence)
        {
            ped.Task.PerformSequence(sequence);
            while (Game.Player.Character.TaskSequenceProgress < 0) await BaseScript.Delay(100); // Wait for the sequence to start
            while (Game.Player.Character.TaskSequenceProgress > 0) await BaseScript.Delay(100); // Wait for the sequence to end
        }

        //public static async Task RunTaskSequence(this Ped ped, TaskSequence sequence, int timeout)
        //{
        //    ped.Task.PerformSequence(sequence);
        //    DateTime endTime = DateTime.Now.AddMilliseconds(timeout);
        //    Client.Log($"Task End Time: {endTime}");
        //    while (Game.Player.ActiveCharacter.TaskSequenceProgress < 0 || DateTime.Now <= endTime) await BaseScript.Delay(100); // Wait for the sequence to start
        //    while (Game.Player.ActiveCharacter.TaskSequenceProgress > 0 || DateTime.Now <= endTime) await BaseScript.Delay(100); // Wait for the sequence to end
        //    if (DateTime.Now > endTime)
        //    {
        //        Client.Log("Force ending task!");
        //        ped.Task.ClearAllImmediately();
        //    }
        //}
    }
}
