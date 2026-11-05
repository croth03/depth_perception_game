# Depth Perception

![Depth Perception — Reality, Hallucination, and Lost Reality](Docs/ReferenceImages/depth-perception-state-moodboard.png)

**Depth Perception** is a short atmospheric true-3D platform game made in Unity. The player moves through one valley as its visual logic shifts from Reality to Hallucination to Lost Reality. Side-scrolling traversal, camera-relative depth movement, voice markers, the distant Figure, and a recurring Door carry the experience.

> **Trust the controls. Question the world.**

## Play and watch

- **Windows build:** [Download Depth Perception](https://github.com/croth03/depth_perception_game/releases/download/v1.0-submission/DepthPerception_Windows.zip)
- **47-second trailer:** [Watch in the repository](Docs/Trailer/DepthPerception_Trailer.mp4) · [Release download](https://github.com/croth03/depth_perception_game/releases/download/v1.0-submission/DepthPerception_Trailer.mp4)
- **Public repository:** [github.com/croth03/depth_perception_game](https://github.com/croth03/depth_perception_game)

The `v1.0-submission` GitHub Release is published. The Windows ZIP was built and smoke-tested locally; both release downloads are publicly listed with verified upload checksums.

To run from source instead, open the project with Unity `6000.6.7f1`, open `Assets/DepthPerception/Scenes/MainMenu.unity`, and enter Play Mode.

## Controls

### SideScroll

- `A / D` or `Left / Right`: move along the side-scrolling path
- `Space`: jump

### DepthTraversal

- `W / S` or `Up / Down`: move forward/back relative to the camera
- `A / D` or `Left / Right`: strafe relative to the camera
- `Space`: jump

Control modes change automatically during authored camera transitions.

## Reward–Damage–End gameplay triad

**Depth Perception** fulfills the Reward–Damage–End gameplay triad through a psychological rather than traditional combat-focused structure.

### Reward

The reward is progression itself. Successfully navigating the level, reaching each psychological marker, hearing its accompanying voice reflection, and moving through Reality, Hallucination, and Lost Reality gradually reveals the story and brings the player closer to the final Door.

### Damage

Damage is expressed mechanically and thematically. Mechanically, mistakes such as failed jumps, falling, or leaving the intended traversal space cause the player to fail and restart from an appropriate checkpoint, creating a clear consequence for unsuccessful play. Thematically, the level explores self-reflection, doubt, distorted perception, and the difficulty of moving forward while remaining trapped by past thoughts. The world becoming less stable visually represents that internal struggle.

### End

The definitive win condition occurs when the player completes the journey and physically walks through the Door. The existing completion fade leads to the final message, **“Good luck on your journey.”** Failure states recover without requiring the Unity Editor or application to restart.

## Tripo assets used in the shipped level

Tripo-generated assets are visibly and functionally incorporated rather than stored unused:

- **Player character:** the controllable protagonist.
- **Stone Door:** functional end-of-level objective and win trigger.
- **Psychological stone markers/totems:** repeated narrative transition objects tied to voice lines and perceptual changes.
- **Ghostly Figure/Specter:** recurring psychological presence during the level progression.
- **Four rock assets:** reused for terrain dressing, platform support, obstacles, and visual grounding.
- **Four tree assets:** environmental scale, framing, silhouette, and depth.
- **Four plant assets:** placed around rocks, paths, markers, and terrain to integrate the playable route into the valley.

Route: `tripo3d.ai directly / generate-and-import → FBX → Unity → URP materials/prefabs → Level01_Valley`.

Tripo provenance, imported filenames, generation summaries, and plan/licensing cautions are documented in [PROMPT_LOG.md](PROMPT_LOG.md), [Docs/TRIPO_PROMPT_LOG.md](Docs/TRIPO_PROMPT_LOG.md), and [REFERENCE_ITEMS_AND_ASSET_ATTRIBUTION.md](REFERENCE_ITEMS_AND_ASSET_ATTRIBUTION.md).

## Audio

Sound effects, ambience, music, and synthetic voice lines were generated with **ElevenLabs** and integrated through reusable Unity audio systems. The sound palette includes valley wind, distant birds, footsteps, clothing movement, breathing, jump/landing sounds, Figure presence, Door ambience, perception transitions, state music, and psychological voice reflections.

The attribution document records the remaining requirement to preserve the ElevenLabs account tier and generation-time license evidence.

## AI production tools

- **OpenAI Codex (GPT-5-based agent):** C# implementation, Unity scene/prefab authoring, documentation, diagnostics, and iterative fixes.
- **Tripo Studio:** 3D meshes, PBR textures, humanoid rig/animation data, environment assets, Figure, markers, and Door.
- **ElevenLabs:** sound effects, music, ambience, and synthetic voice.
- **OpenAI image generation through Codex:** original submission moodboards and generated trailer key art.
- **Unity 6.6:** final engine integration, URP rendering, Cinemachine cameras, Input System, physics, UI, audio, build, and gameplay capture.

## Documentation

- [Master AI Prompt Log](PROMPT_LOG.md)
- [Reward–Damage–End Gameplay Triad](REWARD_DAMAGE_END.md)
- [Reference Items & Asset Attribution](REFERENCE_ITEMS_AND_ASSET_ATTRIBUTION.md)
- [Game Vision](Docs/GAME_VISION.md)
- [Art Bible](Docs/ART_BIBLE.md)
- [Visual Language](Docs/VISUAL_LANGUAGE.md)
- [Level 01 Design](Docs/LEVEL_01_VALLEY.md)
- [Tripo Prompt Log](Docs/TRIPO_PROMPT_LOG.md)
- [Submission Checklist](SUBMISSION_CHECKLIST.md)

## Project structure

```text
Assets/DepthPerception/
  Animations/       Player animator and animation assets
  Art/              URP materials and generated supporting textures
  Audio/            Reusable audio prefabs
  Backdrop/         Valley depth, cloud, fog, and background systems
  Prefabs/          Player, Door, Figure, story, atmosphere, environment
  Scenes/           MainMenu, Level01_Valley, EndScreen
  Scripts/          Gameplay, camera, transitions, audio, atmosphere, UI flow
  Sound/            ElevenLabs-generated MP3 files
  Tripo/            Source Tripo FBX and texture folders
Docs/               Persistent design and attribution documentation
Packages/           Unity package manifest/lock files
ProjectSettings/    Unity project settings and build-scene configuration
```

Unity-generated folders such as `Library/`, `Temp/`, `Logs/`, `UserSettings/`, and packaged builds are excluded through the standard Unity `.gitignore`. Large 3D, image, audio, and video formats are configured for Git LFS.

## Build order

1. `MainMenu`
2. `Level01_Valley`
3. `EndScreen`

`START GAME` loads Level 01. Completing the Door sequence loads the End Screen. `RETURN TO START` returns to the Main Menu.

## Credits and licensing

Game concept, creative direction, human playtesting, and final editorial decisions: **project creator**.

AI-assisted content and tooling are disclosed in [REFERENCE_ITEMS_AND_ASSET_ATTRIBUTION.md](REFERENCE_ITEMS_AND_ASSET_ATTRIBUTION.md). In particular:

- Tripo free-plan models are public and are not assumed commercially cleared without paid/private generation evidence.
- ElevenLabs rights depend on the plan active when each audio file was generated.
- OpenAI-generated output is used subject to OpenAI's terms and human review.
- Unity and its official packages remain subject to Unity's applicable licenses.

No commercial game's art, characters, levels, music, logos, or code were intentionally imported or reproduced.

