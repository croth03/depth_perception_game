# Level 01 — The Valley

Scene: Assets/DepthPerception/Scenes/Level01_Valley.unity

## Purpose

A compact first production level that establishes movement, real 3D depth, the Figure, and the progression from Reality to Hallucination to a short Lost Reality introduction.

Target eventual traversal after population and polish: approximately 3–5 minutes.

## Section A — Reality / Valley Entrance

- Player begins at the low end of a grounded valley.
- Route uses slopes, terraces, rocks, short gaps, and one or two simple jumps.
- Valley walls, ridges, and layered silhouettes create large spatial depth.
- The Figure stands high and far away without a UI marker.
- The route draws the player beneath and toward the Figure.

## First perception jump

A larger broken section passes through a dense fog bank.

During the jump:
- Cinemachine blends away from the conventional side view.
- The camera orbits around the player and reveals genuine depth.
- Movement direction blends from world X to world Z.
- Fog density and color shift.
- The player lands in the same valley viewed from a new orientation.

Primary Inspector tuning:
- Camera blend duration.
- Movement-axis blend duration.
- Fog target color and density.
- Trigger bounds and checkpoint.

## Section B — Hallucination Valley

- Echoes the same stone palette and repeated terrain motifs.
- Uses overlapping silhouettes, offset repeated rocks, ambiguous gaps, and a route along the newly revealed depth direction.
- Movement and collision stay trustworthy.
- The Figure becomes physically closer as the player advances.

## Figure encounter

The encounter briefly softens player control, brings the Figure through real 3D space, strengthens fog, and connects Figure/player composition. It then blends to Lost Reality, places the player above the next route for a short fall, changes the movement axis, and returns control.

Primary Inspector tuning:
- Figure approach duration.
- Lost Reality camera blend duration.
- Fog color/density.
- Lost spawn and checkpoint.
- Input lock duration.

## Section C — Lost Reality

A short, readable introduction with unsupported structures, repeating arches at contradictory depths, a diagonal camera, and a luminous unsupported Door as endpoint.

## Future passes

Do not add final flora, fauna, Tripo assets, audio, or elaborate cinematics yet. Next work should refine traversal distances, camera blend framing, terrain silhouettes, and environmental population after visual evaluation.
