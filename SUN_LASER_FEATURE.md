# Sun Laser Firing Feature

## Overview
This feature enables the Sun object in the game to fire laser beams at Leaf objects after a specified time delay.

## Components

### 1. LaserBeam.cs
A script that handles the visual representation and lifetime of a laser beam.

**Features:**
- Automatically configures a LineRenderer component
- Self-destructs after a configurable lifetime (default: 0.5 seconds)
- Provides a `SetPositions()` method to set start and end points

**Public Variables:**
- `lifetime` (float): Duration the laser remains visible before being destroyed

### 2. Sun.cs (Updated)
Enhanced with laser firing capabilities while preserving the original level completion trigger functionality.

**New Public Variables:**
- `delay` (float): Time delay in seconds before firing lasers (default: 5 seconds)
- `laserPrefab` (GameObject): Reference to the Laser prefab used for instantiation
- `firePoint` (Transform): Optional transform from which lasers are fired. If null, uses the Sun's position

**Functionality:**
- Uses a Coroutine (`FireLaserAfterDelay`) to introduce a delay before firing
- Finds all objects tagged as "Leaf" using `GameObject.FindGameObjectsWithTag("Leaf")`
- Instantiates laser beams from the sun/fire point to each leaf position
- Prevents multiple laser firings with a `hasFiredLaser` flag

### 3. Laser Prefab
A prefab located at `Assets/Prefabs/Laser.prefab` that contains:
- LineRenderer component with yellow color gradient
- LaserBeam script component
- Configured to create a visual laser effect

## Setup Instructions

1. **Assign the Laser Prefab:**
   - Select the Sun object in your scene
   - In the Inspector, find the "Laser Settings" section
   - Drag the `Laser.prefab` from `Assets/Prefabs/` to the `Laser Prefab` field

2. **Configure Delay (Optional):**
   - Adjust the `Delay` value in the Inspector to change how long before lasers fire
   - Default is 5 seconds

3. **Set Fire Point (Optional):**
   - If you want lasers to originate from a specific point, create an empty GameObject as a child of the Sun
   - Assign this transform to the `Fire Point` field
   - If left empty, lasers will fire from the Sun's center position

4. **Ensure Leaf Objects are Tagged:**
   - The Leaf prefab has been updated to use the "Leaf" tag
   - Any existing Leaf instances in scenes may need to be manually tagged

## How It Works

1. When the game starts, the Sun's `Start()` method initiates the `FireLaserAfterDelay()` coroutine
2. The coroutine waits for the specified delay time
3. After the delay, `FireLasersAtLeaves()` is called
4. This method finds all active GameObjects with the "Leaf" tag
5. For each leaf found, a laser beam is instantiated
6. The laser beam's start position is set to the fire point (or Sun position)
7. The laser beam's end position is set to the leaf's position
8. Each laser automatically destroys itself after its lifetime expires

## Code Structure

The implementation follows Unity best practices:
- Clear separation of concerns (LaserBeam handles visualization, Sun handles firing logic)
- Well-commented code with XML documentation
- Public variables exposed in Inspector for easy customization
- Use of Coroutines for time-based behavior
- Proper null checks and safety validation

## Technical Details

- **Delay Mechanism:** Uses `WaitForSeconds` in a Coroutine (as requested in requirements)
- **Target Detection:** Uses `GameObject.FindGameObjectsWithTag("Leaf")`
- **Laser Visualization:** Uses Unity's LineRenderer component
- **Reusability:** All key parameters are configurable via public variables
- **Module Separation:** LaserBeam.cs is a separate, reusable component

## Testing

To test the feature:
1. Open the scene containing the Sun object
2. Ensure the Laser prefab is assigned in the Sun's Inspector
3. Add some Leaf objects to the scene (or use existing ones)
4. Make sure Leaf objects are tagged with "Leaf"
5. Enter Play mode
6. After 5 seconds (or your configured delay), lasers should fire from the Sun to all active Leaves
7. Lasers should disappear after 0.5 seconds

## Customization Options

You can customize the laser appearance by modifying the Laser prefab:
- Change LineRenderer width
- Modify the color gradient
- Adjust the laser lifetime
- Add particle effects or other visual enhancements
