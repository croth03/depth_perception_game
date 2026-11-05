# Depth Perception — Master AI Prompt Log

Last updated: November 4, 2026

This is a concise production record rather than a transcript. I summarized what I asked the coding agent to do, what changed in Unity, and how later playtests corrected earlier assumptions. The original long-form prompts are intentionally not repeated here.

## Chronological production log

### 1. Unity connection and tool check

I first asked the agent to connect to the open Unity project, report the active project and scene, check Play Mode and the Console, and confirm that it could inspect scenes, GameObjects, scripts, and Play Mode controls without modifying anything. I then used a temporary `Codex_Test` cube and rotation script to prove that the connection could create, save, run, and verify a small change. This was a tool test, not part of the final game.

### 2. Initial game scaffold and Level 01 greybox

I established **Depth Perception** as a short atmospheric true-3D platformer built around the rule: “Trust the controls. Question the world.” The first scene became `Level01_Valley`, with SideScroll movement, jumping, camera-driven perspective changes, Reality/Hallucination/Lost Reality states, the distant Figure, checkpoints, fall recovery, and a Door objective. Early prompts deliberately prioritized a playable greybox over art and audio.

### 3. SideScroll and DepthTraversal mechanics

I asked for two automatic control modes. SideScroll kept A/D movement on a constrained lane, while DepthTraversal used camera-relative WASD on the horizontal X/Z plane with normalized diagonals. The camera and transition triggers were responsible for switching modes; the player never manually toggled them. Jumping, gravity, grounding, collision, and recovery had to remain dependable in both modes.

### 4. Runtime control diagnosis after human playtesting

The first automated report said third-person movement worked, but my playtest showed the player was still trapped at the transition. I asked the agent to treat the human test as authoritative and inspect the live movement mode, transition lock, Rigidbody constraints, Z clamps, camera state, and actual keyboard input. The corrective pass removed lingering side-scroll restrictions during DepthTraversal, ensured controls unlocked after the blend, and settled the camera behind and above the player. A temporary mode display made `SIDE SCROLL`, `DEPTH TRAVERSAL`, and `LOCKED` visible during testing.

### 5. Level 01 changed from “open valley” to mechanics introduction

After the movement concept worked, I shortened Level 01 into a tutorial-like sequence: SideScroll, jump, first depth transition, short third-person passage, return to SideScroll, Figure/Lost Reality transition, final traversal, and Door. The larger natural valley was moved to the Level 02 plan. I also asked for clearer player visibility, fewer foreground obstructions, reachable final platforms, and a functional Door that locked movement, faded out, displayed completion, and held a clean end state.

### 6. Persistent art direction and production rules

I asked the agent to consolidate the visual direction in `AGENTS.md`, `GAME_VISION.md`, `ART_BIBLE.md`, `VISUAL_LANGUAGE.md`, and `LEVEL_01_VALLEY.md`. The important production rule became “one asset library, three interpretations”: Reality presents believable objects, Hallucination rearranges or contradicts them, and Lost Reality pushes the same place into a stronger but still composed visual grammar. Tripo was defined as an asset generator, not a level designer.

### 7. Backdrop, atmosphere, and final Door approach

I requested a reusable valley backdrop with distant ridges, fog layers, clouds, and sparse birds. The Door was moved deeper into the scene so the ending reused the established DepthTraversal language: the player sees it from SideScroll, the camera turns behind the character, WASD activates, and the player walks through the doorway from the front. This pass also introduced state-specific atmosphere while preserving the same recognizable valley silhouettes.

### 8. Tripo environment integration

I asked the agent to turn the floating-platform greybox into a grounded valley using the existing Tripo rocks, trees, plants, and stone Door. Gameplay colliders and platform positions stayed authoritative; visual-only meshes disguised platform sides as cliffs, shelves, roots, ridges, and outcrops. Materials were rebuilt for URP, normal maps were corrected, and reusable prefabs were created. Later prompts reinforced that Hallucination and Lost Reality should reinterpret the same environment rather than delete it.

### 9. Production player and animation iterations

I replaced the placeholder player with several Tripo humanoid candidates while preserving the existing controller and simple collider. The first imports exposed rig, ground-offset, root-motion, and retargeting problems: floating feet, sinking into terrain, bent knees, arms held forward, swiveling legs, and clips that sometimes failed to play. I tested the original character, a rigged re-export, `player model`, `player model 01`, `main girl`, and `main girl 02`. The two “main girl” attempts did not solve the problems, so I explicitly reverted to the stable pre-`main girl` setup. The retained production visual is `player model 01`, using a Humanoid Avatar, supplied clips, in-place motion, and the gameplay controller as the source of movement.

### 10. Story markers, Figure, audio, and atmosphere systems

I asked for reusable psychological stone markers with one-shot voice triggers, ambience ducking, and a carved-symbol glow synchronized to actual voice playback. The Tripo ghostly humanoid replaced the placeholder Figure without gaining collision or enemy behavior. A lightweight audio director organized music, ambience, voice, player sounds, Figure presence, and Door sounds. Distant birds, moths, moving clouds, and perception-state treatments were added without changing traversal.

### 11. Visual correction passes driven by video and screenshots

I supplied `game1.mp4` and screenshots to identify issues that were not obvious from code inspection. The corrections focused on floating natural geometry, muddy Lost Reality readability, tree and foreground obstruction, an unwanted horizontal line behind the Figure, oversized rock artifacts, marker grounding/orientation, cloud motion, and lighting separation. I repeatedly narrowed the scope when broad fixes were inefficient.

### 12. Focused cloud correction

The cloud layer visibly cut through the playable ground and created an unnatural line where the ground changed tone. I asked for a single-purpose correction: move the clouds to background-only placement and prevent them from intersecting the playfield. No gameplay or level-layout changes were authorized in that pass.

### 13. Start and end flow

Finally, I requested a minimal atmospheric `MainMenu` with `DEPTH PERCEPTION` and `START GAME`, plus an `EndScreen` with “Good luck on your journey.” and `RETURN TO START`. A small reusable scene navigator now handles `MainMenu → Level01_Valley → EndScreen → MainMenu`. The existing Door trigger and fade were preserved; the end screen loads only after the walk-through completion sequence finishes.

## Tripo generation summary

All recorded Tripo assets used the route **`tripo3d.ai directly / generate-and-import`**, followed by FBX import, inspection, URP material setup, prefab creation, and scene placement. The original Tripo credit balances before and after each generation were not saved. That is a documentation gap: future generations should record the starting balance, ending balance, number of variants, and whether texture, rigging, or animation credits were used.

| Order | Asset/folder | Short prompt summary | Production result |
|---|---|---|---|
| 1 | `stone door` | A monumental freestanding rectangular doorway made from dark, weathered stone; human-scale opening; eerie and restrained; no attached building, magic runes, or ornate fantasy decoration. | Integrated as the functional final Door and reusable prefab. |
| 2 | `player character 3d model` | A believable young woman with long dark hair, a dark weathered coat with long tails, muted clothing, and a clear human silhouette suitable for side and third-person views. | Early visual candidate; later replaced. |
| 3 | `rock formation 01–04` | Four complementary dark eroded valley-rock shapes: broad shelves, layered outcrops, fractured vertical cliffs, and irregular broken masses. | All four became reusable visual-only prefabs and were used to ground the greybox. |
| 4 | `tree model 01–04` | Sparse, windswept, believable valley trees with restrained foliage and readable silhouettes rather than a dense fantasy forest. | All four became reusable visual-only prefabs for foreground, middle distance, and altered-state echoes. |
| 5 | `plants 01–04` | Low, muted rocky-valley vegetation in several shapes, designed to sit at rock bases and ledge margins without hiding traversal. | All four became reusable visual-only prefabs. |
| 6 | `player character 3d model rig` | A rigged/animation-ready version of the dark-coated main character with standard humanoid proportions. | Tested after the first character lacked usable animation; deformation and locomotion remained unsatisfactory. |
| 7 | `player model` | Revised humanoid player model with embedded rig/animation data and the same restrained dark silhouette. | Imported and evaluated, then superseded. |
| 8 | `player model 01` | Improved production humanoid with long dark coat/hair and supplied idle, walk, run, and jump clips for Unity Humanoid retargeting. | Current retained production player visual and animation source. |
| 9 | `stone slab 01–03` | Ancient, weathered standing stone markers with a simple carved psychological symbol; readable, restrained, and suitable for story triggers. | `stone slab 03` became the reusable Psychological Marker; the other variants remain source options. |
| 10 | `ghostly humanoid 3d model` | An ambiguous distant human-like Figure: pale/translucent, unsettling but not monstrous, with a simple readable silhouette. | Integrated as the visual Figure with translucent material, slow sway, moth/smoke treatment, and no gameplay collider. |
| 11 | `main girl` | Alternate production heroine with embedded humanoid animations, intended to improve natural locomotion. | Tested, but animation data was incomplete or unsuitable; not retained. |
| 12 | `main girl 02` | Second alternate heroine using light sway, walk-in-place, jump, moving jump, and landing clips. | Tested with a simplified controller, but did not resolve the movement problems; reverted. |

Full imported filenames, FBX paths, prefab paths, and the original long Door prompt remain in `Docs/TRIPO_PROMPT_LOG.md`.

## Audio prompt summary

The audio files were generated with **ElevenLabs** outside Unity and manually imported into `Assets/DepthPerception/Sound`. The repository does not record the exact ElevenLabs model, account tier, credit balance, or complete original wording, so I am documenting the concise prompt intent preserved by the filenames and production brief rather than claiming details that were not saved. Free-plan ElevenLabs output is non-commercial and requires attribution; paid-plan commercial rights depend on the applicable Terms and the plan active when the audio was generated.

| Audio group | Short prompt summary | Result |
|---|---|---|
| Valley wind | Cold, sparse mountain-valley wind; subdued, atmospheric, and loop-friendly. | Four variations imported. |
| Distant birds | Sparse distant bird calls with long gaps; natural scale and no busy forest chorus. | Four variations imported. |
| Stone footsteps | Human walking footsteps on stone, grounded and restrained. | Four variations imported. |
| Running footsteps | Faster human footfalls on stone without exaggerated action-game impact. | Four variations imported. |
| Jump takeoff | Short human effort/takeoff sounds suitable for a normal jump. | Four variations imported. |
| Landing | Short human landing impacts with body/clothing weight, not a heavy crash. | Four variations imported. |
| Clothing movement | Subtle coat and fabric movement for a long-jacketed character. | Four variations imported. |
| Idle breathing | Quiet young-adult breathing for occasional idle use. | Four variations imported. |
| Walking breathing | Light movement breathing for occasional traversal use. | Four variations imported. |
| Figure presence | Low, uncanny, restrained presence tone for the distant Figure. | Four variations imported. |
| Perception transition | Slow psychological transition texture, atmospheric rather than glitch-heavy. | Multiple variations imported. |
| Door ambience | Ancient stone-Door ambience with a low, uncertain presence. | Three variations imported. |
| Door crossing | Short restrained transition sound for physically crossing the Door. | Three variations imported. |
| State music | Minimal atmospheric pieces for Reality, Hallucination, and Lost Reality, moving from still valley ambience toward stronger afterimage/psychological texture. | `Lonely Valley Drift`, `Valley in Stillness`, `Valley Afterimage(s)`, and `Lost Reality` tracks imported. |
| Voice lines | Calm, intimate, ambiguous internal dialogue about looking back, waiting, doubt, memory, ground, and perception. | Multiple short voice clips imported and assigned to story markers. |

No dedicated insect sound was available; moths remained visual-only. No new audio was generated during the later cleanup passes.

## Trailer and shot prompts

I prepared a 47-second trailer that combines original AI-generated key art with human-recorded Unity gameplay from `game1.mp4`. No generative-video model was used: the AI images are treated as restrained image-to-video shots through editorial pan/zoom, while the playable footage remains an authentic capture of the game.

| Shot | Short prompt / editorial direction | Planned use |
|---|---|---|
| Opening key art | Create an original cinematic triptych of the same valley shifting through Reality, Hallucination, and Lost Reality: misty blue naturalism, restrained violet fracture, then dark geometric psychological distortion; no copied commercial-game imagery. | 0–3 second hook with the title `DEPTH PERCEPTION` and the line “Trust the controls. Question the world.” |
| SideScroll gameplay | Use the opening side-view traversal from the real Unity capture; prioritize readable movement, jump timing, and the layered valley silhouette. | Establish the player and core platforming. |
| Perspective transition | Cut to the authored camera shift and then the first usable DepthTraversal passage. | Show the game's central perspective mechanic. |
| Figure / Lost Reality | Select the clearest later footage of the distant Figure and the altered environment. | Reveal the psychological escalation without explaining the ending. |
| Door approach | Use the final third-person approach footage. | Establish the objective and provide narrative closure. |
| Closing key art | Create an original split-composition key image showing side-scroll traversal becoming a behind-player path toward the stone Door, with cyan/violet accents and empty center space for type. | Final title card, `PLAY NOW`, and repository call to action. |

The trailer audio uses the project track `Valley_in_Stillness_2026-11-03T030023.mp3`, generated in ElevenLabs, mixed underneath the captured game audio. The finished trailer was exported on November 4, 2026 as `Docs/Trailer/DepthPerception_Trailer.mp4`: 47 seconds, 1920×1080, 30 fps H.264 video with stereo AAC audio. The edit was visually checked through a contact sheet plus dedicated opening and closing frame reviews. The ignored local `Submission/` folder retains the working filter, verification images, and release copy without placing those intermediate files in source control.

## Error recovery log

| Problem observed | How I re-prompted | Resolution |
|---|---|---|
| The camera entered a third-person angle, but the player remained trapped in one-axis movement. | I rejected the code-only verification and required inspection of the live mode, control lock, Rigidbody constraints, Z clamp, velocity, and camera state. | DepthTraversal now removes the side-scroll restriction, unlocks input after the transition, and accepts camera-relative WASD. |
| Foreground structures hid the player in later side-view sections. | I asked for composition and obstruction fixes without flattening the 3D scene. | Blocking geometry was repositioned/resized or moved behind the active traversal plane. |
| The Door looked like an endpoint but did not clearly finish the level. | I asked for a reusable completion trigger, movement lock, fade, completion text, and safe hold. | The player now walks through from the front, completes the fade, and proceeds to `EndScreen`. |
| The imported player floated, sank into the ground, bent her knees unnaturally, held her arms forward, or produced a swiveling leg. | I supplied video evidence and requested animation-only diagnosis of Avatar mapping, root motion, clip import, offsets, and controller transitions. | Several candidates were tested; root motion stayed disabled and the project reverted to the more stable `player model 01` setup. |
| The first replacement character lacked the needed clips. | I canceled that pass and redirected the agent to `player model 01`. | The supplied Humanoid idle/walk/run/jump clips were used instead of synthesizing a fake walk. |
| `main girl` and `main girl 02` still did not animate naturally. | I narrowed the requested clip set, tested explicit idle/walk/jump/landing choices, then asked to revert. | Both experiments were removed from active use and the pre-`main girl` player was restored. |
| Tripo rocks initially made the scene feel like floating platforms with art dropped on top. | I asked the agent to preserve collision but visually connect paths to cliffs, shelves, roots, and ridges. | Gameplay geometry stayed intact while visual-only rock dressing grounded the route. |
| Lost Reality became muddy and large natural artifacts appeared in the sky. | I supplied timed video and screenshot references and separated intentional geometric frames from accidental trees/rocks. | Natural artifacts were corrected while the authored surreal frames remained. |
| A thin horizontal element crossed behind the Figure. | I identified the exact screenshot region and asked for a focused correction rather than a redesign. | The conflicting line/geometry was removed or repositioned without changing the encounter. |
| Clouds did not visibly move and later cut through the ground, changing its tone along a straight line. | I reduced the scope to one instruction: make clouds background-only and keep them out of the playfield. | Cloud geometry was moved behind gameplay and no longer intersects traversal surfaces. |
| Reported fixes did not appear in the Unity instance I was playing. | I asked the agent to identify the exact active project rather than assuming the workspace copy was live. | The connected Editor and production path were confirmed as `C:\Users\thepa\test`, and later changes were verified there. |

## Analytical reflection (200–300 words)

AI handled most of the repetitive Unity implementation: creating scripts and prefabs, wiring scene references, rebuilding URP materials, configuring Humanoid imports, placing repeated environment assets, checking Console output, and running narrow Play Mode tests. It was especially useful when a change touched several systems at once, such as switching movement modes while blending cameras or connecting the Door fade to a new end scene. It also kept persistent documentation so later prompts could be shorter.

My role was not simply to approve its output. I defined the concept, visual rules, emotional tone, level structure, and production limits. More importantly, I performed the human playtests. Several technically plausible reports were wrong in practice: third-person movement existed in code but was not usable, foreground depth blocked the character, animation clips were valid but looked physically unnatural, and clouds were “atmospheric” while visibly cutting through the ground. I used video, screenshots, and direct descriptions to reject those results, narrow the next prompt, and decide when to revert instead of continuing to patch a weak asset.

Working in an engine changed the division of labor compared with PG2 because success could not be judged from code or a static image alone. Unity introduced live state, import settings, physics constraints, camera blends, root motion, clip transitions, colliders, scene references, and build order. The AI could inspect and automate those systems quickly, but I still had to judge whether the game felt readable, natural, and intentional while actually playing it. The most effective workflow was therefore iterative: I set the creative target, AI implemented and instrumented it, I tested the lived result, and the next prompt corrected the gap between technical completion and player experience.
