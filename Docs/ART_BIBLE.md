# Depth Perception — Art Bible

## Purpose

This document is the persistent visual and asset-production source of truth for **Depth Perception**. Read it before work involving characters, the Figure, the Door, rocks, foliage, animals, fog, materials, Tripo-generated assets, or perceptual-state presentation.

The existing [Depth Perception presentation](https://croth03.github.io/depth-perception-site/index.html) remains a conceptual and mood reference: deep blue-black atmosphere, fog-filled negative space, readable silhouettes, recurring motifs, and the progression from recognizable to ambiguous to unbound. Its architecture is not a requirement for every level. Level 02 should prioritize a believable natural valley rather than copying fantasy ruins.

## Core visual promise

The world changes more than the player.

**Trust the controls. Question the world.**

The game should feel quiet, lonely, dreamlike, uneasy, beautiful, large, uncertain, melancholic, and mysterious. Surrealism must feel authored and readable. Reality should be credible and inviting enough that the player wants to explore it before doubt begins.

Avoid:

- bright cartoon fantasy
- generic RPG or ornate fantasy-portal aesthetics
- cyberpunk neon overload
- random surrealism, floating cubes, or meaningless abstraction
- constant glitch effects, camera shake, or visual noise
- gore and conventional fantasy monsters
- tricks that obscure fair movement, collision, or failure feedback
- environments so busy that the player or route becomes hard to read

## Production principle

### One asset library, three interpretations

Do not build independent Reality, Hallucination, and Lost Reality libraries by default.

- **Reality establishes the object.** It is recognizable, physically credible, and conventionally placed.
- **Hallucination changes perception.** Reuse the same object with altered arrangement, repetition, alignment, scale, material response, visibility, wind, or depth cues.
- **Lost Reality changes rules.** Reuse remains preferred, but a small number of genuinely impossible variants or treatments are allowed.

Most state changes should be authored in Unity. A new mesh is justified only when transform, material, duplication, child visibility, or VFX cannot express the intended difference.

## Three-state visual matrix

| Element | Reality | Hallucination | Lost Reality |
|---|---|---|---|
| Player | Stable, believable, and clearly readable | Mostly unchanged; remains the perceptual anchor | May receive subtle changes after the Figure encounter, but stays readable |
| Rocks | Natural dark stone, credible erosion, conventional gravity | Same forms repeated, aligned, subtly scaled, rotated, or placed at ambiguous depth | Unsupported masses, intersections, vertical fractures, and contradictory orientation |
| Plants | Sparse believable valley ecology and coherent wind | Same species with repeated silhouettes, wrong scale, conflicting lean/wind, or misleading depth | Familiar growth becomes structural: hanging roots, impossible direction, black branching forms |
| Fog | Low mist and atmospheric separation | Layered banks conceal, suggest, and confuse distance without hiding gameplay | Fog behaves almost architecturally as walls, shafts, curtains, holes, and negative-space forms |
| Animals | Rare, believable wildlife | Familiar animals behave incorrectly or appear at impossible depth | Mostly absent, fragmented, silhouetted, or briefly visible in impossible locations |
| Door | Distant, rare, recognizable, and subtly impossible | More prominent but spatially questionable | Unnaturally stable compared with the world around it |
| Figure | Tiny, distant, human-like uncertainty | Appears closer without distance behaving normally | May intersect or merge with player, fog, or environment |
| Color | Charcoal, stone, muted vegetation, cool atmospheric blue | Cyan, violet, and muted pink begin leaking into natural color | Strongest controlled departure; never rainbow chaos |
| Geometry | Physically credible | Credible objects arranged incorrectly | Geometry itself can violate ordinary spatial rules |

## Player character

The player is a small, solitary human figure who belongs in Reality and becomes visually out of place as the world changes.

Direction:

- human, gender-neutral or ambiguous presentation
- restrained, believable proportions
- simple dark coat or practical outerwear
- practical clothing and boots
- no weapons, armor, mascot features, or fantasy-hero silhouette
- face need not read strongly at normal gameplay distance
- one subtle identifying accent is acceptable
- readable silhouette and movement at every active camera distance

Do not aggressively distort the player without a deliberate narrative reason. Exact costume details remain open until the player reference pass.

## The Figure

The Figure remains deliberately unexplained. It may suggest an observer, guide, memory, another version of the player, part of the landscape, or something else. Do not settle its identity.

For production efficiency, the Figure may eventually derive from the player mesh, altered through:

- scale or elongated proportions
- posture and animation
- obscured face
- silhouette treatment
- material and light response

This is a production option, not a narrative answer.

## The Door

The Door is a recurring hero object: objective, boundary, promise, and possible trap. Its silhouette should remain recognizable across the game.

Direction:

- simple, strong, intentional silhouette
- architecturally unsupported placement is allowed
- strange through context and stability, not ornate decoration
- not a fantasy portal
- one strong reusable base asset, not unrelated doors for every state

State treatment:

- **Reality:** distant, unexplained, or subtly impossible.
- **Hallucination:** harder to locate spatially or apparently offset from its expected depth.
- **Lost Reality:** fully present and unusually stable while the world around it contradicts itself.

Level 01's next implementation pass should move the Door deeper into the scene and make the player approach it from the front using DepthTraversal. See `Docs/LEVEL_01_VALLEY.md`.

## Rock language

Reality rocks should feel geological rather than decorative:

- dark weathered stone
- basalt-like or volcanic forms
- broken ledges and eroded valley rock
- tall fractured formations
- large natural silhouettes
- forms that can be rotated, scaled, clustered, and partially buried

Initial modular kit:

1. large valley boulder
2. tall fractured rock or spire
3. low rock ledge or outcrop
4. rock wall or cliff module

Hallucination should usually reuse those meshes through repetition, alignment, subtle nonuniform scale, unusual rotation, and duplicated fog silhouettes. Lost Reality may use intersections, unsupported masses, arches that should collapse, vertical fractures, or contradictory continuations. Every impossible treatment should serve a composition or traversal idea.

## Foliage language

Level 02 is an open valley, not a dense fantasy forest. Vegetation begins sparse and atmospheric.

Initial compact kit:

1. grass or reed cluster
2. fern or low ground plant
3. sparse shrub
4. small wind-shaped tree
5. dead or bare tree
6. one distinctive larger valley plant

State treatment:

- **Reality:** believable species, coherent wind, natural growth, and sparse valley ecology.
- **Hallucination:** reuse the same plants with contradictory lean or wind, repeated silhouettes, incorrect scale, misleading depth, and branches emerging from fog.
- **Lost Reality:** familiar growth becomes structural or abstract—hanging roots, impossible growth direction, unsupported growth, enlarged familiar forms, and dark branching masses.

Do not turn plants into generic monsters.

## Animal language

Wildlife is sparse evidence that Reality is alive. The initial future kit is deliberately small:

1. dark bird or raven-like bird
2. moth or small insect swarm
3. distant deer-like animal

Reality behavior is mostly natural. Hallucination changes behavior or spatial evidence rather than turning animals into monsters: repeated silhouettes, impossible distance, inconsistent apparent size, or a flock briefly suggesting the Figure. Lost Reality may reduce animals to partial silhouettes, fragments, absence, or brief impossible appearances.

Do not build wildlife until the player, Door, rocks, foliage, fog, and state-treatment foundation are validated.

## Fog language and planned layers

Fog is a Unity rendering and VFX system, not a Tripo asset and not merely a global density slider. It must support composition without obscuring fair traversal.

### Layer 1 — Atmospheric distance

Global distance fog separates foreground, middle distance, and distant landscape. Reality uses it subtly.

### Layer 2 — Local fog banks

Localized cards, meshes, particles, or VFX sit in valleys and between spatial layers to create depth separation and selective concealment.

### Layer 3 — Transition fog

Denser animated fog supports SideScroll ↔ DepthTraversal transitions by concealing and revealing the physical change in viewpoint.

### Layer 4 — Hallucination fog

Purposefully placed banks suggest figures, conceal and reveal silhouettes, complicate distance judgment, and create false visual relationships without hiding the real route.

### Layer 5 — Lost Reality fog

Fog becomes graphic and nearly architectural: vertical shafts, walls, curtains, voids, holes, and strongly shaped negative space. Keep it elegant and sparse.

Remain on URP. Do not migrate render pipelines solely for fog. Local post-processing volumes and URP-compatible shaders/VFX may support gradual state changes in a later implementation pass.

## Color and material language

The restrained palette in `Docs/VISUAL_LANGUAGE.md` remains authoritative.

- **Reality:** charcoal, weathered stone, muted natural vegetation, cool atmospheric blue, low saturation.
- **Hallucination:** Signal cyan, Drift violet, and muted Doubt pink leak carefully into otherwise natural materials and fog.
- **Lost Reality:** permits the strongest departure, but color stays controlled and purposeful.

Material changes should preserve object identity. Prefer parameter variation and selective overrides over unrelated replacement materials.

## Camera and composition rules

- The world is genuinely 3D; camera changes reveal real depth.
- SideScroll means movement within a perceptual layer.
- DepthTraversal means physical travel between perceptual layers or toward a depth objective.
- Use strong foreground, middle, and background separation without blocking the player or immediate route.
- The player remains the perceptual and readability anchor during major camera transitions.
- The Figure is discovered through composition, never a quest marker.
- The Door should grow naturally through perspective during a depth approach.
- Stable interface and essential feedback never hallucinate.

## Planned Unity state-treatment system

A future reusable component, provisionally named `DepthStateVisual`, should let an object define per-state treatment without requiring separate meshes. Potential controls include:

- material or parameter override
- object and child visibility
- transform, rotation, and scale offsets
- VFX intensity
- emissive or appearance values

This is a design target only. Do not implement it during documentation or asset-generation passes unless explicitly requested.

## Tripo and asset-production rules

Tripo is an asset generator, not a level designer.

Unity remains responsible for gameplay geometry, collision, level composition, navigation, cameras, perspective transitions, and state orchestration.

Preferred asset workflow:

visual concept/reference → game-oriented Tripo mesh → inspect → retopology/poly-budget adjustment → PBR textures → Unity import/Bridge → prefab → simple reliable collider → state-treatment integration

Rules:

- Use deliberate reference imagery rather than text-only generation for hero assets.
- Use consistent multi-view references for the player, Door, and other proportion-sensitive assets.
- A strong single reference may be sufficient for simple rocks and plants.
- Generate and test the smallest useful kit before expanding it.
- Do not ask Tripo to generate complete levels or gameplay terrain.
- Do not create three independent state variants unless a distinct mesh is genuinely required.

## Initial asset kit

Do not expand this list until the first assets are tested in-game.

### Hero assets

- player
- Door

### Rocks

- large valley boulder
- tall fractured rock or spire
- low rock ledge or outcrop
- rock wall or cliff module

### Foliage

- grass or reed cluster
- fern or low ground plant
- sparse shrub
- small wind-shaped tree
- dead or bare tree
- distinctive larger valley plant

### Later wildlife

- dark bird or raven-like bird
- moth or insect swarm
- distant deer-like animal

## Production order

1. Implement the Level 01 Door depth-approach revision.
2. Validate and maintain this Art Bible.
3. Design and produce the player asset.
4. Design and produce the Door asset.
5. Produce and test the four-piece modular rock kit.
6. Produce and test the compact foliage kit.
7. Build the improved layered fog system in Unity.
8. Implement the reusable state-driven visual treatment system.
9. Produce the small wildlife kit.
10. Build Level 02 — The Valley from the validated reusable library.

## Efficiency and repository rules

- Reuse transforms, materials, clustering, partial burial, duplication, and child variants before generating more meshes.
- Do not build a large environment before its visual rules and modular kit are validated.
- Keep persistent decisions in project documents rather than repeatedly rebuilding context in prompts.
- Before significant FBX/GLB files, large textures, PSDs, or audio enter the repository, configure the planned Git LFS workflow.
- Do not rewrite Git history merely to begin planning assets.

## Current boundary

This Art Bible records direction. It does not authorize asset generation, Level 02 construction, a fog implementation, wildlife work, or movement-system redesign. Each production phase should be requested and validated separately.
