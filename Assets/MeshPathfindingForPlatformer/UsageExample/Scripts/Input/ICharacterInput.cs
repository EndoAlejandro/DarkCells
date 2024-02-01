/*
 * This is an interface so that a character can switch between player input or use AI
 */

namespace Calcatz.Example {
    public interface ICharacterInput {

        bool Enabled { get; set; }

    }
}
