"use client";

import Image from "next/image";
import Link from "next/link";

/* ─── GCV-style Tilted Mosaic Hero
   9 columns of images, rotated ~20°, opacity 0.4 over black bg.
   Centered headline + subtitle + CTA overlay on top.
   Matches gcventures.vc hero layout exactly. ─── */

const MOSAIC_IMAGES = [
  /* col 1 */ "/media/hero/hero-dunk.jpg", "/media/lifestyle/trophy-celebration.jpg", "/media/gallery/G1_01_Dunk_Action.jpg", "/media/lifestyle/coach-mentorship.jpg",
  /* col 2 */ "/media/gallery/G2_01_Coach_Portrait_Blue_Wall.jpg", "/media/lifestyle/youth-action.jpg", "/media/gallery/G1_08_Team_Huddle_Celebration.jpg", "/media/hero/hero-crowd.jpg",
  /* col 3 */ "/media/lifestyle/defensive-play.jpg", "/media/gallery/G1_15_Coach_Clipboard.jpg", "/media/lifestyle/game-face.jpg", "/media/gallery/G2_08_Intense_Coaching_Moment.jpg",
  /* col 4 */ "/media/gallery/G1_05_Crowd_Spectator.jpg", "/media/lifestyle/spectators-family.jpg", "/media/hero/hero-fastbreak.jpg", "/media/gallery/G2_12_Group_Bleachers.jpg",
  /* col 5 */ "/media/lifestyle/three-point-shot.jpg", "/media/gallery/G1_20_Halftime_Celebration.jpg", "/media/lifestyle/award-ceremony.jpg", "/media/gallery/G2_18_Free_Throw_Action.jpg",
  /* col 6 */ "/media/gallery/G1_12_Spectator_Group.jpg", "/media/lifestyle/layup-contested.jpg", "/media/hero/hero-huddle.jpg", "/media/gallery/G2_05_Three_Staff_Celebration.jpg",
  /* col 7 */ "/media/lifestyle/team-celebration.jpg", "/media/gallery/G1_25_Ready_Stance.jpg", "/media/lifestyle/crowd-energy.jpg", "/media/gallery/G2_25_Award_Presentation.jpg",
  /* col 8 */ "/media/gallery/G1_30_Fast_Break.jpg", "/media/lifestyle/tournament-vibe.jpg", "/media/gallery/G2_28_Ready_Stance_Action.jpg", "/media/lifestyle/basketball-action.jpg",
  /* col 9 */ "/media/gallery/G1_17_Rebounding_Action.jpg", "/media/lifestyle/youth-skills.jpg", "/media/gallery/G2_15_Packed_Crowd_Scene.jpg", "/media/lifestyle/game-intensity.jpg",
];

const COLS = 9;
const PER_COL = 4;

export default function HeroSection() {
  const columns: string[][] = [];
  for (let c = 0; c < COLS; c++) {
    columns.push(MOSAIC_IMAGES.slice(c * PER_COL, c * PER_COL + PER_COL));
  }

  return (
    <section className="mosaic-hero" aria-label="Hero">
      {/* Tilted image mosaic — background layer, opacity 0.4 */}
      <div className="mosaic-bg">
        <div className="mosaic-tilt">
          <div className="mosaic-grid">
            {columns.map((col, ci) => (
              <div key={ci} className="mosaic-col">
                {col.map((src, ri) => (
                  <div key={ri} className="mosaic-tile">
                    <Image
                      src={src}
                      alt=""
                      fill
                      sizes="200px"
                      style={{ objectFit: "cover" }}
                      aria-hidden="true"
                      priority={ci < 5}
                    />
                  </div>
                ))}
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* CTA overlay — centered, z-index above mosaic */}
      <div className="mosaic-content">
        <div className="mosaic-text">
          <div className="mosaic-actions">
            <Link href="/register" className="btn-hero btn-hero-primary">
              Register Now
            </Link>
            <Link href="/locations" className="btn-hero btn-hero-secondary">
              Explore Tournaments
            </Link>
          </div>
        </div>
      </div>
    </section>
  );
}
