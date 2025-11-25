# marbles

"כדורי הזכוכית" הוא משחק דו‑שחקני תחרותי שמדמה את משחק הגולות הקלאסי בממדי מחשב. כל שחקן מנסה לזרוק את הכדור שלו אל חור במרכז, ואם מצליח — מרוּשה ניסיון לפגוע בכדור היריב ולהרויח את שניהם. המשחק מדגיש דיוק, טיימינג, החלטה אסטרטגית ומתח רגעי.

פלטפורמה: המחשב האישי (מקלדת ועכבר) ובנוסף גרסת מגע לנייד — חוויית משחק חוצה‑פלטפורמות.

to play the game:https://yousef-masarwa97.itch.io/marbles



<img width="1536" height="1024" alt="image" src="https://github.com/user-attachments/assets/0545fa9a-94fd-4ec8-94b3-45ac661983fb" />


לרכיבים הרשמיים לחצו כאן:

https://github.com/Game-Dev-Course-ArielUNI/marbles/wiki


---

```mermaid
classDiagram
    direction TB

    class GameManager {
        +static GameManager Instance
        +SimpleDragNewInput[] players
        +int currentPlayerIndex
        +GamePhase phase
        +Awake() void
        +CurrentPlayer() SimpleDragNewInput
        +OnShotFinished(enteredHole bool, hitEnemy bool) void
        +SwitchTurn() void
    }

    class SimpleDragNewInput {
        -Camera cam
        -Rigidbody2D rb
        -bool dragging
        -Vector3 startPos
        -float maxDrag
        -float launchPower
        -int playerIndex
        -bool enteredHole
        -bool insideHole
        -bool touchedHoleThisShot
        -bool hitEnemy
        +Awake() void
        +Update() void
        +CheckStopped() void
        +OnTriggerEnter2D(col Collider2D) void
        +OnTriggerExit2D(col Collider2D) void
        +OnCollisionEnter2D(col Collision2D) void
    }

    class GamePhase {
        <<enumeration>>
        HoleShot
        AttackShot
        Waiting
        GameOver
    }

    GameManager --> SimpleDragNewInput : manages players *
    SimpleDragNewInput --> GameManager : uses singleton
    GameManager --> GamePhase : controls state


