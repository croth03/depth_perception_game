# Tripo Prompt Log

## Asset 1 — Stone Door

- **Route:** `tripo3d.ai directly / generate-and-import`
- **Imported filename:** `tripo_convert_695ce564-cea7-485a-9518-d9b049574d5f.fbx`
- **Asset path:** `Assets/DepthPerception/Tripo/stone door/tripo_convert_695ce564-cea7-485a-9518-d9b049574d5f.fbx`
- **Unity prefab:** `Assets/DepthPerception/Prefabs/Door/StoneDoor.prefab`
- **Generation:** Smart Mesh / P2, triangle topology, approximately 13,051 faces / 6,566 vertices.
- **Generation prompt:** “Monumental freestanding doorway for a dark atmospheric game called Depth Perception. A tall rectangular stone doorway standing completely alone in a natural valley, with no wall or building attached. Approx. 3.5–4.5 meters tall, with an opening comfortably large enough for a human character to walk through. Realistic, minimal, restrained, eerie, melancholic, believable. Thick rectangular frame, open center passage, fully modeled front and back, strong silhouette, slightly irregular, dark weathered stone or aged concrete, subtle cracks and erosion. No walls, stairs, buildings, glowing runes, magic effects, fantasy carvings, sci-fi elements, ornate gothic decoration, bright colors, or solid closed door. Game-ready asset, clean topology, suitable for Unity.”
- **Pipeline:** `Tripo Smart Mesh/P2 → texture/PBR → FBX export → Unity import → URP material/prefab/collider setup`
- **Placement:** `Level01_Valley` functional final Door. Player approaches it using DepthTraversal and physically walks through it to complete the level.

## Asset 2 — Production Player

- **Route:** `tripo3d.ai directly / generate-and-import`
- **Imported filename:** `tripo_convert_3ec10d3c-4ceb-4908-9437-42185047a2ab.fbx`
- **Asset path:** `Assets/DepthPerception/Tripo/player model 01/tripo_convert_3ec10d3c-4ceb-4908-9437-42185047a2ab.fbx`
- **Imported mesh:** 45,967 vertices / 52,902 triangles, one SkinnedMeshRenderer.
- **Rig status:** Valid Unity Humanoid Avatar with 67 skinned bones and bind poses. The FBX supplies seven genuine Humanoid takes: Idle, Walk, Run, Jump, JumpStraightUp, WalkJumpForward, and RunJumpForward. The full Jump take is preserved as `JumpFull`; a non-destructive frames 42–72 `Jump` subclip removes its padded standing frames for gameplay. The production controller uses the supplied Idle, Walk, Run, and trimmed Jump motion while the existing gameplay controller remains authoritative and root motion stays disabled.
- **Pipeline:** `Tripo Smart Mesh → texture/PBR → animated FBX → Unity Humanoid import → supplied locomotion clips → visual prefab → existing player controller`
- **Placement:** Main playable character used throughout the game. The visual is installed on the existing `Player/Visual` root while gameplay continues to use the existing `CharacterController` and movement systems.

## Asset 3 — Valley Rock Kit

- **Route:** `tripo3d.ai directly / generate-and-import`
- **Pipeline:** `Tripo Smart Mesh → texture/PBR → FBX → corrected normal-map import → restrained URP material → reusable visual-only prefab → Level01_Valley dressing`
- **Placement:** All four rocks are reused throughout Reality, Hallucination, and Lost Reality. Approximately 59 visual-only placements disguise platform sides, form cliff shelves and outcrops, connect traversal structures toward the valley basin, and embed the final Door without changing gameplay collision.

| Variant | Actual source FBX | Unity prefab | Level 01 function |
|---|---|---|---|
| Rock 01 | `Assets/DepthPerception/Tripo/rock formation 01/tripo_convert_dd89129b-f771-4c28-8fe8-803149081289.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Rock_01.prefab` | Broad shelf masses and repeated valley supports. |
| Rock 02 | `Assets/DepthPerception/Tripo/rock formation 02/tripo_convert_cc694248-5ddf-4198-b267-30c5c1c1e8c7.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Rock_02.prefab` | Layered outcrops, corridor banks, and basin forms. |
| Rock 03 | `Assets/DepthPerception/Tripo/rock formation 03/tripo_convert_fa9ad8b1-f00a-40b6-acdd-29f805d1b5bf.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Rock_03.prefab` | Tall fractured cliffs and stronger Lost Reality masses. |
| Rock 04 | `Assets/DepthPerception/Tripo/rock formation 04/tripo_convert_0c8152cf-78d7-43aa-bff7-3e8d253bed6b.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Rock_04.prefab` | Irregular shelf breaks, Door-side embedding, and perceptual echo forms. |

## Asset 4 — Valley Tree Kit

- **Route:** `tripo3d.ai directly / generate-and-import`
- **Pipeline:** `Tripo Smart Mesh → texture/PBR → FBX → corrected normal-map import → muted URP material → reusable visual-only prefab → Level01_Valley silhouette/depth placement`
- **Placement:** All four tree assets appear in approximately 18 sparse placements. They frame Reality, establish scale in the middle distance, repeat with altered alignment in Hallucination, and become larger contradictory silhouettes in Lost Reality. No tree is used as gameplay collision.

| Variant | Actual source FBX | Unity prefab | Level 01 function |
|---|---|---|---|
| Tree 01 | `Assets/DepthPerception/Tripo/tree model 01/tripo_convert_9b8cb8ec-0192-4d69-a566-3a5cd0b52a92.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Tree_01.prefab` | Natural near/midground scale reference. |
| Tree 02 | `Assets/DepthPerception/Tripo/tree model 02/tripo_convert_0c3513e7-65dd-4f66-b390-156cf2603f6b.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Tree_02.prefab` | Windswept framing and repeated depth evidence. |
| Tree 03 | `Assets/DepthPerception/Tripo/tree model 03/tripo_convert_bf6e4ebd-8a0c-4dc3-b8a4-117bcc48d080.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Tree_03.prefab` | Fuller valley silhouettes behind active traversal. |
| Tree 04 | `Assets/DepthPerception/Tripo/tree model 04/tripo_convert_82ee4a02-3670-43e1-b982-5165b16ed167.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Tree_04.prefab` | Sparse dark silhouette and state-colored echo placements. |

## Asset 5 — Valley Plant Kit

- **Route:** `tripo3d.ai directly / generate-and-import`
- **Pipeline:** `Tripo Smart Mesh → texture/PBR → FBX → corrected normal-map import → muted URP material → reusable visual-only prefab → restrained ledge clusters`
- **Placement:** All four plant assets appear in approximately 28 small clusters around rock bases, safe platform margins, transition areas, and the Door approach. They visually join rock formations to the level while keeping jump surfaces, triggers, and the Door passage clear.

| Variant | Actual source FBX | Unity prefab | Level 01 function |
|---|---|---|---|
| Plant 01 | `Assets/DepthPerception/Tripo/plants 01/tripo_convert_3de028b4-4365-4ad9-be59-e99d999e1e86.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Plant_01.prefab` | Rocky ground clusters and shelf-edge breakup. |
| Plant 02 | `Assets/DepthPerception/Tripo/plants 02/tripo_convert_849fe7a7-6253-4a78-974d-162fe17a70a8.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Plant_02.prefab` | Muted low foliage around ledges and passage margins. |
| Plant 03 | `Assets/DepthPerception/Tripo/plants 03/tripo_convert_51dae135-7136-4d41-b435-c54eb5065873.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Plant_03.prefab` | Natural vegetation accents at tree and rock bases. |
| Plant 04 | `Assets/DepthPerception/Tripo/plants 04/tripo_convert_8f65b432-55a4-4727-a5a5-38ad13f1af4d.fbx` | `Assets/DepthPerception/Prefabs/Environment/Tripo/Tripo_Plant_04.prefab` | Drier clusters for sparse and altered-state areas. |

## Asset 6 — Psychological Stone Marker

- **Route:** `tripo3d.ai directly / generate-and-import`
- **Imported filename:** `tripo_convert_fece5369-b293-4903-8da7-521c43d783f8.fbx`
- **Asset path:** `Assets/DepthPerception/Tripo/stone slab 03/tripo_convert_fece5369-b293-4903-8da7-521c43d783f8.fbx`
- **Unity prefab:** `Assets/DepthPerception/Prefabs/Story/PsychologicalMarker.prefab`
- **Pipeline:** `Tripo Smart Mesh → texture/PBR → FBX → restrained URP material → configurable visual/trigger prefab → per-instance voice and transition settings`
- **Placement:** Two independently configured Level 01 story markers. The slab is visual-only; a simple trigger volume drives one-shot voice playback and optional state/camera behavior without changing traversal collision.

## Asset 7 — Ghostly Humanoid Figure

- **Route:** `tripo3d.ai directly / generate-and-import`
- **Imported filename:** `tripo_convert_3a90191e-edcf-48cf-9747-58e85f6e3668.fbx`
- **Asset path:** `Assets/DepthPerception/Tripo/ghostly humanoid 3d model/tripo_convert_3a90191e-edcf-48cf-9747-58e85f6e3668.fbx`
- **Unity prefab:** `Assets/DepthPerception/Prefabs/Figure/GhostFigure.prefab`
- **Pipeline:** `Tripo skinned source mesh → FBX → translucent URP material → restrained smoke/moth treatment → slow Unity-authored sway/opacity → authored Figure encounter`
- **Placement:** Replaces the Level 01 placeholder Figure while preserving the existing encounter sequence. It remains a visual/narrative object with no gameplay collider, navigation, or new character-animation system.
