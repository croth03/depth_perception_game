# Depth Perception — Reference Items & Asset Attribution

Last reviewed: November 4, 2026

This document records the project's concept references, mood and gameplay targets, generative-AI toolchain, and asset-license status. It is a production record, not legal advice. Terms can change, so the linked official terms should be checked again before public or commercial release.

## 1. Concept inspiration and target experience

### Original concept source

- **Depth Perception presentation:** [croth03.github.io/depth-perception-site](https://croth03.github.io/depth-perception-site/index.html)
- **Role:** Original project concept, mood, recurring symbols, and early spatial-mechanics reference.
- **Authorship:** Project creator's own presentation.
- **Important production change:** The website predates the final technical direction. The Unity game is now a genuine 3D environment presented mainly through side-scrolling movement; it is not a flat three-plane game.

### Concept and mood language

The target is a quiet, lonely, dreamlike, uneasy, beautiful, melancholic landscape. A recognizable valley gradually becomes less trustworthy while controls, collision, and player feedback remain dependable. The recurring Door, distant Figure, fog, fractured stone, sparse vegetation, and large negative spaces carry the mood.

The core production rule is:

> **Trust the controls. Question the world.**

The three perceptual states are different readings of the same place:

- **Reality:** believable geology, grounded scale, cool blue-gray atmosphere, and subtle doubt.
- **Hallucination:** the same forms repeat, overlap, drift, or contradict depth; restrained cyan and violet appear.
- **Lost Reality:** the same valley accepts impossible orientation, unsupported masses, and stronger violet/magenta/cyan relationships while traversal remains readable.

### Original state moodboard

![Original three-state valley moodboard showing Reality, Hallucination, and Lost Reality](Docs/ReferenceImages/depth-perception-state-moodboard.png)

*Original documentation image generated with OpenAI image generation through Codex on November 4, 2026. It visualizes the project's established Art Bible; it is not copied from a commercial game and is not currently used as an in-game asset.*

## 2. Gameplay references and camera language

The mechanical reference is the broad tradition of side-scrolling platform games that use foreground/background depth as part of navigation. No commercial game's characters, levels, art, audio, logos, or code were imported or intentionally reproduced.

The final Unity implementation differs from the early browser prototype in a major way:

- **SideScroll** means movement along a constrained path inside one perceptual layer.
- **DepthTraversal** means camera-relative WASD movement through real X/Z space.
- Camera rotation reveals genuine 3D depth; the world is not flattened into discrete visual planes.
- Perspective transitions are both mechanical teaching moments and narrative events.
- The player remains readable and physically trustworthy even when the environment becomes uncertain.

### Original gameplay-composition reference

![Original gameplay composition comparing side-scrolling movement with third-person depth traversal](Docs/ReferenceImages/depth-perception-gameplay-composition.png)

*Original documentation image generated with OpenAI image generation through Codex on November 4, 2026. The left side visualizes SideScroll composition; the right side visualizes the slightly elevated behind-player DepthTraversal camera and Door approach.*

## 3. Target look

### Visual priorities

- Grounded, sculptural dark stone rather than visible floating greybox platforms.
- Strong foreground, middle-distance, and background separation without blocking the player.
- Sparse windswept trees and plants rather than a dense fantasy forest.
- Layered fog that supports depth and transitions without intersecting gameplay surfaces.
- A small, solitary, dark-coated human silhouette.
- A simple unsupported rectangular stone Door, strange because of context rather than ornament.
- An ambiguous distant Figure discovered through composition, never through a quest marker.
- Cool moonlike light and a restrained palette: Ink, Night, Stone, Mist, Signal cyan, Drift violet, and Doubt pink.

### Deliberate exclusions

The target is not bright cartoon fantasy, ornate gothic portal art, cyberpunk neon, random floating cubes, constant glitch effects, gore, generic monsters, or a visually noisy environment that hides traversal.

The complete visual rules are maintained in:

- `Docs/ART_BIBLE.md`
- `Docs/VISUAL_LANGUAGE.md`
- `Docs/GAME_VISION.md`
- `Docs/LEVEL_01_VALLEY.md`

## 4. Target sound

The sound direction is sparse, physical, and psychological rather than loud or conventionally cinematic.

- Cold valley wind provides the continuous environmental bed.
- Distant birds make Reality feel alive without creating a busy forest soundscape.
- Stone footsteps, clothing movement, breathing, jump effort, and landings keep the player physically present.
- The Figure uses a low restrained presence rather than a monster sting.
- The Door has an ancient low ambience and a short crossing response.
- Voice lines are intimate, calm, and ambiguous, as though memory or self-doubt is entering the environment.
- Reality music is still and natural; Hallucination introduces afterimage-like instability; Lost Reality becomes more psychologically abstract without turning into noise.

### Original soundscape moodboard

![Original soundscape moodboard visualizing wind, breath, footsteps, birds, the Figure, and Door ambience](Docs/ReferenceImages/depth-perception-soundscape-moodboard.png)

*Original documentation image generated with OpenAI image generation through Codex on November 4, 2026. It visualizes the intended listening experience; it is not an audio asset.*

The detailed text summaries of the audio-generation prompts are in `PROMPT_LOG.md`.

## 5. Complete generative-AI tool inventory

| Area | Tool/model | How it was used | Output in this project | Attribution/license status |
|---|---|---|---|---|
| Code, documentation, Unity authoring, diagnosis | **OpenAI Codex**, GPT-5-based coding agent in the Codex desktop app | Created and edited C# scripts, scene/prefab wiring, documentation, material/import setup, runtime checks, and debugging under human direction. | Project code, documentation, editor-authored scene changes, and implementation assistance. | OpenAI's current terms state that, as between the user and OpenAI and to the extent permitted by law, the user owns Output. Inputs remain the user's. Output may not be unique, and code output may be subject to third-party/open-source licenses. Human review remains required. |
| Documentation images | **OpenAI image generation through Codex**; exact hosted image-model build is not exposed in the tool result | Generated the three original reference boards embedded in this document from the project's own Art Bible and mechanics. | `Docs/ReferenceImages/*.png` | Covered as OpenAI Output; documentation-only. No commercial game or named artist was used as a style target. |
| 3D meshes, PBR textures, rigging, and supplied animation | **Tripo Studio / tripo3d.ai** | Generated/imported the Door, player candidates, production player, rocks, trees, plants, stone markers, and ghostly Figure. | FBX, texture, rig, and animation data under `Assets/DepthPerception/Tripo/`; Unity prefabs/materials derived from those sources. | **Plan at generation is not recorded. Treat every Tripo asset as free-plan/public and not commercially cleared until paid/private generation evidence is attached.** See Section 7. |
| Tripo generation model | **Smart Mesh / P2 documented for the Door**; exact Tripo model/version for most other assets is not preserved | Produced game-oriented source meshes before Unity cleanup. | Door generation record plus other Tripo FBX folders. | Version metadata is incomplete. Do not claim that every model used P2 without account/export evidence. |
| Sound effects, ambience, music, and voice | **ElevenLabs** — Sound Effects, Music, and text-to-speech/voice generation; exact model names and settings not recorded | Generated multiple variants of wind, birds, footsteps, movement, breathing, Figure/Door sounds, state music, and spoken lines, then manually imported them. | MP3 files in `Assets/DepthPerception/Sound/`. | Provider confirmed. **Account tier and generation-time license remain unresolved.** Free-plan output is non-commercial and requires ElevenLabs attribution; paid-plan output generally carries commercial rights subject to the applicable Terms, Music Terms, and Prohibited Use Policy. |
| Trailer/video | **OpenAI image generation through Codex + human Unity capture; no generative-video model used** | Original AI key art was used as restrained pan/zoom image-to-video shots and combined with the creator's `game1.mp4` gameplay capture. | `Docs/Trailer/DepthPerception_Trailer.mp4`; key art in `Docs/ReferenceImages/`; local intermediates in ignored `Submission/`. | OpenAI-generated stills are disclosed; gameplay capture is creator-owned. |
| Installed but not evidenced as a content-generation source | Unity AI Assistant `2.21.0-pre.2` and Unity AI Inference `2.6.1` | Present in `Packages/manifest.json`, but no project record shows them producing shipped content. | Installed packages only. | Do not list them as asset creators unless later usage is documented. |

## 6. Asset Attribution Sheet

### 6.1 Project-authored and OpenAI-assisted content

| Asset/category | Location | Creator/source | License/rights note | Release status |
|---|---|---|---|---|
| Original C# gameplay and editor scripts | `Assets/DepthPerception/Scripts/` | Project creator with OpenAI Codex assistance | Project-owned output as between user and OpenAI; review for copied/open-source fragments remains the user's responsibility. No third-party code source is recorded. | Cleared subject to normal code review. |
| Unity-authored scenes, prefabs, materials, VFX, and primitive geometry | `Assets/DepthPerception/Scenes/`, `Prefabs/`, `Art/`, `Backdrop/`, `Audio/` | Project creator with Codex/Unity tooling | Original project content. Unity states that creators retain ownership of content created with Unity; Unity software/packages remain under their own terms. | Cleared subject to referenced source assets below. |
| Reference moodboards in this document | `Docs/ReferenceImages/` | OpenAI image generation via Codex, prompted from the project's original design language | OpenAI Output; user owns Output as between user and OpenAI, subject to applicable law and third-party rights. Documentation-only. | Cleared for documentation; keep AI disclosure. |
| Original concept presentation | `https://croth03.github.io/depth-perception-site/` | Project creator | Creator-owned reference. | Cleared. |
| Unity playtest video and screenshots | User-provided local captures | Project creator | Creator-owned gameplay and diagnostic media; not generative video. | Cleared if the capture contains only authorized project content. |
| Trailer key art and title-card imagery | `Docs/ReferenceImages/depth-perception-state-moodboard.png`, `depth-perception-gameplay-composition.png` | OpenAI image generation through Codex, directed from the project's original art language | OpenAI Output; animated only through editorial pan/zoom, not a generative-video model. | Cleared for the coursework trailer with AI disclosure. |

### 6.2 Tripo-generated 3D source inventory

All items below use the recorded route `tripo3d.ai directly / generate-and-import`. Their Unity materials, prefabs, colliders, and placements are derivative project setup; the underlying mesh/texture/rig/animation rights depend on the Tripo plan and privacy status at the moment of generation.

| Tripo source folder | Contents/role | Current use | License action |
|---|---|---|---|
| `stone door` | Freestanding weathered stone Door | Final Level 01 Door prefab | Verify paid/private generation. Otherwise label as public free-plan Tripo output and do not assume commercial clearance. |
| `player model 01` | Current production humanoid player, textures, rig, supplied clips | Active player visual | Same Tripo verification required. |
| `player character 3d model` | Early player candidate | Retained source, not active | Same verification required while included in repository/build distribution. |
| `player character 3d model rig` | Rigged early player candidate | Retained source, not active | Same verification required. |
| `player model` | Alternate player candidate | Retained source, not active | Same verification required. |
| `main girl` | Experimental alternate player | Reverted/not active | Same verification required or remove from release package if unnecessary. |
| `main girl 02` | Second experimental alternate player | Reverted/not active | Same verification required or remove from release package if unnecessary. |
| `rock formation 01–04` | Four valley geology variants | Active environment prefabs | Same verification required. |
| `tree model 01–04` | Four sparse valley-tree variants | Active environment prefabs | Same verification required. |
| `plants 01–04` | Four low vegetation variants | Active environment prefabs | Same verification required. |
| `stone slab 01–03` | Psychological marker variants | Slab 03 is active; others retained | Same verification required. |
| `ghostly humanoid 3d model` | Ambiguous Figure source mesh and textures | Active Figure prefab | Same verification required. |

Imported filenames and the production mapping are recorded in `Docs/TRIPO_PROMPT_LOG.md` and `PROMPT_LOG.md`.

### 6.3 Audio inventory

| Audio category | Files | Source/creator | License status | Required action |
|---|---|---|---|---|
| Valley wind and distant birds | Multiple MP3 variants in `Assets/DepthPerception/Sound/` | ElevenLabs Sound Effects | Plan/model unverified | Record the ElevenLabs account tier, generation date, applicable Terms, and credit receipt. |
| Walking/running footsteps, jump, landing, clothing, idle/walking breathing | Multiple MP3 variants | ElevenLabs Sound Effects | Plan/model unverified | Same; confirm that any human vocal sounds use authorized stock/generated voices rather than an unauthorized clone. |
| Figure presence, perception transition, Door ambience/crossing | Multiple MP3 variants | ElevenLabs Sound Effects | Plan/model unverified | Same. |
| Reality/Hallucination/Lost Reality music | `Lonely_Valley_Drift`, `Valley_in_Stillness`, `Valley_Afterimage(s)`, `Lost_Reality` MP3 files | ElevenLabs Music | Plan/model unverified | Record the generation-time plan and verify the game's permitted media category under the ElevenLabs Music Terms. |
| Spoken psychological lines | Short quote-named MP3 files | ElevenLabs text-to-speech/voice generation | Voice/model and plan unverified | Record the ElevenLabs voice/model used, confirm the voice was premade or otherwise authorized, and retain the commercial-license evidence. |

ElevenLabs is now confirmed as the source. Until the generation-time plan, voice/model choices, and applicable terms are documented, these files should still be treated as **coursework/prototype assets only**, not commercially cleared assets.

### 6.4 Unity and package dependencies

The project uses Unity 6000.6.7f1 and official Unity packages including URP 17.6.0, Cinemachine 6.6.0, Input System 1.20.1, uGUI 2.6.0, and Unity Pipeline 0.9.1-exp.1. These are software dependencies rather than imported art assets. They remain governed by Unity's applicable software, package, and companion licenses. Preserve package notices and do not redistribute Unity package source as if it were original project content.

No third-party Asset Store art pack, commercial font, stock photograph, or downloaded commercial-game asset was found in the documented production inventory. If one is added later, record the publisher, product URL, invoice/license, seat count, modification status, and exact project location here.

## 7. Tripo terms and free-plan disclosure

### Required disclosure

**Models generated under Tripo's free plan are public.** Current Tripo pricing describes free models as public and non-commercial. Tripo's current terms state that Tripo retains extensive rights, including intellectual-property rights, in free-user inputs and outputs. Public visibility does not mean the project automatically owns exclusive rights or has unrestricted commercial permission.

Current official references:

- [Tripo Terms of Service](https://developers.tripo3d.ai/en/terms)
- [Tripo Pricing & Credits](https://www.tripo3d.ai/product-doc/getting-started/pricing-credits)
- [Tripo pricing comparison](https://www.tripo3d.ai/pricing)
- [Tripo commercial-use guidance](https://www.tripo3d.ai/help/privacy-policy/how-to-use-tripo-models-commercially)
- [Tripo data/privacy guidance](https://www.tripo3d.ai/help/privacy-policy/how-to-understand-data-privacy-and-ai-training-at-tripo)

As of this review, the official pages distinguish roughly as follows:

| Plan/status | Visibility | Ownership/rights summary | Commercial-use summary |
|---|---|---|---|
| Free user/output | Public under current plan language | Tripo's Terms state that Tripo retains the rights described for free-user inputs and outputs. Some official feature pages also reference attribution licensing; where wording differs, follow the stricter Terms and account-specific license. | Do not assume commercial permission. Current pricing labels free models non-commercial and requiring attribution. |
| Paid user, private output | Private under current paid-plan language | Current guidance states that paid users hold rights in their inputs/outputs, while granting Tripo the service license described in its terms. | Current guidance permits commercial use, subject to the active plan, privacy setting, source rights, and full Terms. |

### Project-specific Tripo status

The repository does **not** currently contain invoices, account-plan screenshots, generation receipts, privacy-state records, or per-asset license certificates. For safe attribution:

1. Treat every Tripo source as **free-plan, public, and not commercially cleared** until proven otherwise.
2. Credit **Tripo AI / tripo3d.ai** for the generated meshes, textures, rigs, and supplied animations.
3. Before commercial release, attach paid-plan/private-generation evidence for each active asset or regenerate the active set under a qualifying paid/private plan.
4. Record whether each source image or prompt was original and whether any third-party character, trademark, or copyrighted reference was used.
5. Re-check the Terms immediately before submission or release.

Suggested coursework credit:

> Selected 3D meshes, textures, rigs, and animations were generated with Tripo AI and then imported, configured, materially rebuilt, composed, and integrated in Unity. Tripo assets generated on a free plan are public; ownership and permitted use remain subject to Tripo's current Terms of Service and the generation plan used.

## 8. ElevenLabs terms and audio disclosure

The project audio was generated with **ElevenLabs**. The repository does not yet record whether those files were generated on the Free, Starter, Creator, Pro, or another plan, nor does it preserve the exact Sound Effects, Music, or voice-model settings.

Current ElevenLabs guidance distinguishes free and paid use:

- Free-plan output is for personal, non-commercial use and requires attribution to ElevenLabs.
- Paid plans generally provide commercial-use rights for audio generated during the paid subscription, subject to the Terms and Prohibited Use Policy.
- ElevenLabs states that a commercial license for content generated during a qualifying subscription continues after that subscription ends.
- ElevenLabs Music has additional plan- and use-specific terms, including game categories, download rights, distribution restrictions, and possible attribution requirements.
- Synthetic voice output must use a voice the user is authorized to use; the project must not rely on an unauthorized cloned identity.

Official references:

- [ElevenLabs Terms of Use](https://elevenlabs.io/terms-of-use)
- [ElevenLabs pricing and commercial-license plans](https://elevenlabs.io/pricing)
- [ElevenLabs Music Terms](https://elevenlabs.io/music-terms)
- [ElevenLabs subscription-end license guidance](https://elevenlabs.io/docs/help-center/account/general/what-happens-to-my-content-after-my-subscription-ends)

Until the account tier is confirmed, the safest submission credit is:

> Sound effects, music, ambience, and synthetic voice audio created with ElevenLabs. Use is subject to the ElevenLabs plan and terms in effect when each file was generated.

If the files were generated on the Free plan, include a visible ElevenLabs attribution and do not treat them as commercially cleared. If they were generated during a paid subscription, save the invoice/account record and generation dates with this document.

## 9. OpenAI and Unity terms notes

OpenAI's current terms say that, as between the user and OpenAI and to the extent permitted by law, the user retains input rights and owns output. They also warn that output may not be unique. OpenAI's Service Terms additionally note that Codex/code-generation output can be subject to third-party licenses.

- [OpenAI Terms of Use](https://openai.com/policies/row-terms-of-use/)
- [OpenAI Service Terms](https://openai.com/policies/service-terms/)

Unity states that creators retain ownership of their project content and that Unity does not claim project revenue, while use of the Editor and packages remains subject to Unity's licenses and terms.

- [Unity licensing and content ownership FAQ](https://unity.com/pages/license-compliance)
- [Unity Terms of Service](https://unity.com/legal/terms-of-service)
- [Unity Companion License](https://unity.com/legal/licenses/unity-companion-license)

## 10. Release-readiness checklist

- [ ] Record the Tripo account plan and public/private status used for every active FBX.
- [ ] Save Tripo receipts or account screenshots showing generation date and applicable rights.
- [ ] Regenerate or replace any active free-plan Tripo asset if commercial/private rights are required.
- [x] Identify the audio, music, and voice generation service: ElevenLabs.
- [ ] Record the ElevenLabs account tier and generation dates for every MP3 group.
- [ ] Confirm commercial/game-distribution rights under both the general ElevenLabs Terms and Music Terms.
- [ ] Record the ElevenLabs voice/model used and confirm that it was premade or otherwise authorized.
- [ ] Preserve OpenAI, Tripo, and Unity terms links with a final review date.
- [ ] Add any future stock, Asset Store, font, texture, music, or reference-image license before importing it.
- [ ] Keep generated moodboards labeled as AI-generated documentation references.
- [ ] Re-check the full build for unused experimental Tripo/audio assets before release.

