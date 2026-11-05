# Depth Perception — Game Vision

## One-line premise

A short atmospheric 3D platform game in which the world becomes increasingly unreliable while movement, collision, and player feedback remain trustworthy.

## Production shape

- Approximately 3–5 small authored levels.
- Real 3D environments with predominantly side-scrolling movement.
- Perspective camera; camera movement is a gameplay and narrative device.
- Deliberate staging over breadth, procedural sprawl, or large content libraries.
- Core promise: **Trust the controls. Question the world.**

## Spatial gameplay language

- **SideScroll** means the player is moving within a perceptual layer.
- **DepthTraversal** means the player is physically travelling between perceptual layers or toward an objective deeper in the scene.
- Camera transitions reveal genuine 3D space; they do not substitute a flat plane or rotate the world as a trick.

## Level roles

### Level 01 — Mechanics Introduction

Level 01 is a short, comparatively abstract introduction to side-scrolling, jumping, perspective change, DepthTraversal, the Figure, the Door, and movement between perceptual states. It is not responsible for delivering the full open-valley concept.

### Level 02 — The Valley

Level 02 will be the first major environmental showcase. It will introduce an open natural valley with believable terrain, strong foreground/middle/background composition, atmospheric fog, vegetation, eventual wildlife, distant Figure observation, and larger exploration compositions. Reality should initially appear genuinely normal before the journey moves physically deeper into Hallucination.

Level 02 should be built only after the reusable player, Door, rock, foliage, fog, and state-treatment foundation has been established.

## Asset philosophy

Use **one asset library with three interpretations**. Reality establishes recognizable objects; Hallucination changes their arrangement, repetition, alignment, scale, material response, visibility, or depth cues; Lost Reality permits more aggressive treatment and a small number of genuinely impossible variants. Most differences should be authored in Unity rather than generated as unrelated meshes.

Detailed visual and asset direction lives in `Docs/ART_BIBLE.md`.

## Perceptual states

### Reality

About 95% believable. Natural scale, gravity, grounded landscape, and readable traversal. Fog conceals but generally does not lie. One or two details may feel impossible.

Player question: **What did I just see?**

### Hallucination

The same place remains recognizable, but visual evidence conflicts. Depth is harder to judge, aligned shapes separate when the camera changes, and fog suggests forms that may not exist. Collision and controls remain reliable.

Player question: **Can I trust what I am seeing?**

### Lost Reality

Ordinary spatial logic is no longer required, but the world develops a new internally consistent grammar. Impossible geometry can become functional and perspective can contradict normal expectations.

Player question: **What rules still work?**

These states are narrative and perceptual spaces, not medical diagnoses.

## Recurring motifs

### The Figure

A distant human-like silhouette whose identity is never settled. It may read as observer, guide, memory, landscape, another self, or something else. It begins far away and later behaves in spatially impossible ways.

### Fog

- Reality: hides.
- Hallucination: suggests.
- Lost Reality: may create form.

Fog supports atmosphere without erasing playable readability.

### The Door

A recurring uncertain objective, sometimes standing without architectural support. It is mechanically dependable even when its meaning and placement are not.

## Design constraints

- No huge levels.
- No random floating-cube surrealism.
- No constant glitch effects or cyberpunk neon overload.
- No gore or definitive explanation of the Figure.
- No visual trick that makes input, collision, or failure feel unfair.
- Every surreal element should be composed and motivated.
