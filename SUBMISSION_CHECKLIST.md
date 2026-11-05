# Portfolio Game 3 — Submission Checklist

Last audited: November 4, 2026

Public repository: [https://github.com/croth03/depth_perception_game](https://github.com/croth03/depth_perception_game)

## Game requirements

- [x] Unity project with responsive SideScroll and DepthTraversal controls.
- [x] Reward documented as narrative/mechanical progression through markers, states, and the Door.
- [x] Damage/penalty documented through failed traversal and checkpoint recovery.
- [x] Definitive win: complete the Door sequence and reach `EndScreen`.
- [x] Recovery does not require restarting the Unity Editor.
- [x] Main Menu and Start Game flow.
- [x] End Screen and Return to Start flow.
- [x] ElevenLabs audio integrated.
- [x] More than three visibly used Tripo assets.

## Required repository contents

- [x] Standard Unity `.gitignore` excludes `Library/`, `Temp/`, `Logs/`, `UserSettings/`, and packaged builds.
- [x] Git LFS rules cover FBX/GLB/Blend/PSD plus MP3/MP4/PNG/JPG/WAV.
- [x] Unity project source organized under `Assets/DepthPerception/`.
- [x] `PROMPT_LOG.md` contains chronological prompts, Tripo/audio summaries, recovery history, and reflection.
- [x] `REWARD_DAMAGE_END.md` preserves the final gameplay-triad explanation and active Tripo inventory.
- [x] `REFERENCE_ITEMS_AND_ASSET_ATTRIBUTION.md` contains inspiration, moodboards, tool inventory, and license sheet.
- [x] `README.md` contains controls, gameplay triad, Tripo inventory, documentation, and release links.
- [x] Commit and push the current production project and documents (`1a87380`).

## Build

- [x] Produce a clean Windows standalone build.
- [x] Launch the packaged build outside the Editor (15-second standalone smoke test completed November 4, 2026).
- [ ] Verify `MainMenu → Level01_Valley → Door → EndScreen → MainMenu`.
- [x] Zip as `DepthPerception_Windows.zip` (135,487,395 bytes; SHA-256 `0805F7E829DEB10746F2D1DF0CEF1E3F938A0D4D73635B62514C97A2F89980EF`).
- [x] Upload to GitHub Release `v1.0-submission`.
- [x] Confirm the public repository and published release resolve while signed out; release assets are listed as uploaded with matching checksums.

Build audit note: the standalone player launched and remained stable during the smoke test. Its log contained no gameplay exception or compilation failure. It did contain blocked Unity analytics/network requests and warnings that optional Gaussian/Bokeh Depth of Field and Panini Projection shaders were stripped from the player; these were not interruption errors.

## Trailer

- [x] Produce a 30–60 second trailer (47 seconds).
- [x] Open with a 0–3 second visual hook.
- [x] Include AI-generated image-to-video/key-art shots.
- [x] Include real Unity gameplay footage.
- [x] Show SideScroll, perspective transition, DepthTraversal, Figure/Lost Reality, and Door.
- [x] End with title and where-to-play call to action.
- [x] Use project audio and maintain a readable mix.
- [x] Record shot prompts and trailer audio usage in `PROMPT_LOG.md`.
- [x] Export as `DepthPerception_Trailer.mp4` (47 seconds, 1920×1080 H.264/AAC; SHA-256 `50F32ABEF426A40201D95C58DE70CCDBF4ACABE6E56337A8C0E2B80E37344A48`).
- [x] Upload to GitHub Release `v1.0-submission` and confirm the README link.

## Attribution and legal record

- [x] Tripo route and asset placement documented.
- [x] Tripo free-plan/public-model warning documented.
- [x] OpenAI/Codex and generated-image use documented.
- [x] ElevenLabs identified as the audio provider.
- [ ] Record the Tripo plan/privacy status used for each active generation.
- [ ] Record the ElevenLabs plan, voice/model choices, and generation dates.
- [ ] Retain receipts/screenshots proving any paid commercial rights.

## Final GitHub/Canvas checks

- [x] Repository is public.
- [x] GitHub Release contains the Windows build and trailer.
- [x] README links resolve from the public repository.
- [x] No `Library/`, root `Temp/`, `Logs/`, `UserSettings/`, local screenshots, or restore snapshots are committed.
- [x] No non-LFS file exceeds GitHub's 100 MB limit.
- [ ] Submit only this URL to Canvas: `https://github.com/croth03/depth_perception_game`

## Optional extra credit

- [x] Publish the build on Itch.io: [croth03.itch.io/depth-perception](https://croth03.itch.io/depth-perception).
- [ ] Add cover artwork, controls, screenshots, and download/play instructions.
- [x] Add the Itch.io URL to `README.md`.
- [ ] Add the Itch.io URL to the Canvas submission comments.

