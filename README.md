# Pit-Popper
Reaction-based Unity game prototype

<img width="1017" height="555" alt="image" src="https://github.com/user-attachments/assets/41c442b4-b209-4b84-b3d0-bcf38ad5f48b" />


**Game design**
After inspecting the assignment requirements, I decided to implement the following approach that in my opinion is a simplified and interesting scenario that captures reaction times of the user.
When the user starts the application, a Start button appears. After clicking it, balls start to fill the pit while moving around. The balls start as green, and they change color, eventually turning red. At this point, they start to flash, a timer has started for the user, he/she needs to click on them (left click) to deactivate them. After a successful deactivation the ball will disappear. A total of 10 balls will appear.
The application records the reaction time of the user from the moment they started flashing.

**Highlights:**
- Abstract input system for easy integration with additional devices (VR headsets, sensors, cameras).
- Reliable data capturing and population of new events with proper interfaces.
- Modular architecture following Unity’s best practices and game design principles.
- Backend service with .NET to upload events and reports.

<img src="https://github.com/user-attachments/assets/7c8c4a5d-dbe4-4c42-9215-1885c1cef7dd" width="600">

**Setup and run instructions**
I provided a .unitypackage to easily import the application to your Unity scene.
I created the package with Unity 6000.1.14f1, to avoid any issues please use this version.

1. Create a new unity project with the Universal 3D template on the Unity version I mentioned above.
2. Once the project loads, navigate to Assets > Import Package > Custom Package and select my package ReactionDemo.
3. Open the scene from Scenes/ ReactionScene. The following scene should appear:

<img width="1299" height="772" alt="image" src="https://github.com/user-attachments/assets/0583f0f2-77ee-41b6-b204-1b8101c38df8" />

4. Click play, everything is configured.
5. Click once to the game window to enable interaction with the mouse and click the Start button. The game will finish when all the balls are spawned (10). You can restart the game by pressing the Restart button.

**Data flow and design choices**
GameManger is responsible for the main flow of the game. It is a singleton that handles when the balls should be spawned and when to start the game. Some game parameters are set there like totalBallsToSpawn, spawnInterval, maxActiveBalls etc. If I had more time
and in a more polished application I would have configured a scriptable object to setup all the game data instead of having it hardcoded to the class.
Each ball has 3 components attached:
- BallMover: Moves the ball around and manages collisions. I chose not to let the balls move with their rigidbody and gravity because it would be very unpredictive.
- BallLifecycle: Handles the colors, the animations and when the ball is ready to be interacted with the user.
- BallStimulus: This class implements the ITrackableStimulus interface that is responsible to track events.

**Tracking events**
I created the ITrackableStimulus interface to make sure all the trackable elements have the same implementation. In this way we can create many trackable events. In our application, I created the BallStimulus event, but we can create more by implementing the ITrackableStimulus interface.
The interface requiresmto implement a method that will fire the event and send it to the DataLogger.
DataLogger is responsible to log the events. When a ball is spawned, the BallStimulus script subscribes to the datalogger by giving a unique ID. Then we can track this event throughout the application. At the end of the scenario, the DataLogger calls the DataExporter that exports the JSON reports.
The reports are saved in the folder Assets/Reports

<img width="461" height="171" alt="image" src="https://github.com/user-attachments/assets/f48f398e-670a-4cc0-a446-2b0f966f0964" />

Each session I save and export 2 reports.
The first one is the reaction_data report that contains all the events the DataLogger captured.

<img width="902" height="502" alt="image" src="https://github.com/user-attachments/assets/4c2aac42-3e0a-4611-9990-ec65ecd8bf4d" />

The second one is the session_report that contains a summary of the session with average numbers, accuracy etc.

<img width="491" height="160" alt="image" src="https://github.com/user-attachments/assets/7d74033c-bdfa-4c62-9674-cd0a9d638f8c" />

**Input System**
I implemented an input system that supports easy integration of additional devices e.g sensors. IInputHandler is an interface that requires for the classes to implement the position of the target on the screen (this can be the position of the mouse, from keyboard, gaze or hand gesture) and an event that fires when the user “clicks” (again this can be implemented with blinking sensors, hand gestures or keyboard etc). For our example I implemented MouseInputHandler that outputs the position of the mouse and fires when the user clicks with the left button. InputManager configures the setup and is a central point for all the calls and events

**Backend API (not in this repo)**
I implemented a simple backend API using .NET that lets users upload their reports.
To use it, navigate to the scene hierarchy and select the DataExporter.

<img width="256" height="328" alt="image" src="https://github.com/user-attachments/assets/295470fd-e615-4540-b02a-97034874bbce" />

From the inspector, enable the “Upload Analytics” option (disabled by default).

<img width="549" height="315" alt="image" src="https://github.com/user-attachments/assets/bfcec7d2-d0c6-456a-adaa-c4d80ae706d9" />

Open a powershell and navigate to the ReactionAPI folder

<img width="1097" height="329" alt="image" src="https://github.com/user-attachments/assets/3e18cee0-f2ac-4a10-818d-9da3c636d13c" />

Run the command dotnet run
This will start the backend

<img width="1462" height="399" alt="image" src="https://github.com/user-attachments/assets/0e8d26d1-0989-44b4-90b7-fcc9dd0701da" />

Play the game once and make sure you reach the Game Over scene.
Open a bowser and navigate to the following urls:
http://localhost:5193/api/reaction/events (for the list of events captured)
http://localhost:5193/api/reaction/summaries (for the summary report)

<img width="713" height="412" alt="image" src="https://github.com/user-attachments/assets/55fe5d8a-e5f0-4034-8a37-aa1db65f5045" />


<img width="691" height="1079" alt="image" src="https://github.com/user-attachments/assets/dc75daa6-246d-43ed-88ed-1031c1b4c953" />


