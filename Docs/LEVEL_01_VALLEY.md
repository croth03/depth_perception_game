# Level 01 — Mechanics Introduction

Scene: `Assets/DepthPerception/Scenes/Level01_Valley.unity`

## Purpose

A short introductory level that teaches two distinct forms of movement while preserving one continuous true-3D environment. This scene is not the large open valley; that role now belongs to Level 02.

The current teaching sequence is deliberately compact:

SideScroll → basic movement and jumping → first perspective transition → short DepthTraversal → SideScroll → Figure transition → final SideScroll → Door → level complete.

The planned final form adds one last short depth approach:

SideScroll → Door visible deeper in space → camera rotates behind the player → DepthTraversal → approach and enter the Door from the front → level complete.

The movement roles remain:

- **SideScroll** means the player is inside a perceptual layer.
- **DepthTraversal** means the player is physically travelling between layers.
- **ControlLocked** is reserved for short, authored camera and Figure transitions.

Reality, Hallucination, and Lost Reality are regions of the same place, not disconnected levels.

## Controls

### SideScroll

- A/D or Left/Right: move along world X.
- Space: jump.
- The controller gently holds the active traversal lane in world Z.

### DepthTraversal

- W/S or Up/Down: move forward and backward relative to the camera on the horizontal ground plane.
- A/D or Left/Right: strafe camera-left and camera-right on the horizontal ground plane.
- Diagonal input is normalized.
- Space: jump where the terrain permits.

## Section A — Reality

The player side-scrolls through the quiet valley entrance along world X. Valley walls, fog, ridges, and distant terrain establish physical depth before the player can enter it. The Figure is visible deeper in the valley without becoming a quest marker.

## Reality → Hallucination journey

The Reality route ends at a fog threshold and broken ridge.

1. Control briefly locks while Cinemachine blends from the Reality side camera to the Valley Depth camera.
2. Fog increases and the player becomes the perceptual anchor.
3. DepthTraversal enables.
4. The player walks physically forward through the existing valley terrain, from approximately world Z 2 to world Z 18.
5. Rock walls and fog constrain the passage without turning it into open exploration.
6. At the destination threshold, the camera rotates back to the Hallucination side camera.
7. SideScroll resumes along world X at the deeper world-Z layer.

Primary Inspector tuning:

- side-to-depth and depth-to-side blend durations
- transition control-lock duration
- depth forward and lateral speeds
- corridor center and half-width
- fog color, density, and blend duration
- passage and arrival checkpoints

## Section B — Hallucination

Hallucination side-scrolls again along world X. It remains recognizably the same valley, now deeper in physical space. Repeated rocks, aligned arches, misleading silhouettes, restrained Drift accents, and uneven shelf spacing make visual evidence less dependable without changing collision rules.

Reality terrain remains behind the player in world space. Lost Reality structures are farther ahead through fog.

## Hallucination → Figure approach

The Hallucination route ends at a gap in the rear valley wall.

1. Control briefly locks while Cinemachine blends to the closer Figure Approach camera.
2. DepthTraversal enables with a narrower lateral corridor.
3. The player walks through real world Z toward the Figure.
4. Repeated terrain and alternating silhouettes make progress feel uncertain while the route remains readable.
5. The Figure encounter trigger occupies deliberate ground near the end of the passage.

The first depth walk teaches the rule. The Figure approach uses the same rule while making distance harder to trust.

## Figure → Lost Reality

At the encounter:

- normal movement enters ControlLocked
- the Figure crosses the remaining physical distance and overtakes the player’s space
- fog strengthens while the camera blends toward the Lost Reality side composition
- the player is displaced through fog to a short, deliberate fall above the Lost Reality landing
- SideScroll is restored along world X only after the player has a valid destination
- the Figure is hidden without explaining what occurred

The Lost Reality spawn sits directly above a widened landing surface. A separate recovery checkpoint is placed on that surface.

## Section C — Lost Reality

Lost Reality returns to side-scrolling presentation. Unsupported paths, contradictory arches, displaced vertical forms, and the recurring Door establish a stronger but still navigable spatial grammar. Foreground structures remain part of the 3D composition, but critical traversal is kept clear from the active side camera.

## Door completion

The Door is the dependable endpoint for this introductory level. The current greybox implementation completes the level from the final side-scrolling path, locks movement, holds the player safely, performs a restrained fade, and displays `LEVEL COMPLETE`.

### Planned Door depth approach

In the next focused implementation pass:

1. Move the Door approximately 8–15 meters deeper in world Z, oriented so its front faces the approach.
2. Keep it clearly visible from the final side-scrolling section.
3. End the supported side-scrolling route before the Door rather than letting the player collide with its side.
4. Automatically rotate the camera into the established third-person composition.
5. Switch the player to `DepthTraversal` with normal camera-relative WASD controls.
6. Let the player walk through real 3D space toward the Door and enter it from the front.
7. Trigger the existing restrained completion fade and `LEVEL COMPLETE` end state only after the player passes through the doorway.

This final approach should reinforce the game's spatial language rather than introduce a new movement system. Level 02 still must not be loaded until it exists.

## Recovery safety

A development recovery volume spans beneath the playable valley. Falling into it restores:

- the most recent checkpoint position
- the movement mode active at that checkpoint
- SideScroll lane or DepthTraversal corridor settings
- the matching side or depth camera presentation
- the current perceptual state

The controller also retains a lower fall-height fallback in case the trigger volume is bypassed.

## Future passes

Do not expand this scene into the open valley or add final flora, fauna, generated assets, audio, enemies, or additional levels here. Future work should preserve its compact teaching role and tune only what hands-on play feedback shows is necessary.
