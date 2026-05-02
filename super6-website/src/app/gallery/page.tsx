import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";

export const metadata: Metadata = {
  title: "Gallery | Super6 Basketball",
  description:
    "Photos from Super6 tournaments across Florida and Atlanta — the courts, the competition, and the moments that matter.",
};

const photos = [
  {
    src: "/media/curated/01-flagship-coach-pointer.jpg",
    alt: "Coach directing players at Super6 event",
    caption: "Coaching in action",
  },
  {
    src: "/media/curated/02-game-action-drive.jpg",
    alt: "Player driving to the basket",
    caption: "Full-speed drives",
  },
  {
    src: "/media/curated/03-crowd-sideline.jpg",
    alt: "Spectators watching from the sideline",
    caption: "Packed sidelines",
  },
  {
    src: "/media/curated/04-team-huddle.jpg",
    alt: "Team huddle during timeout",
    caption: "Team adjustments",
  },
  {
    src: "/media/curated/05-trophy-presentation.jpg",
    alt: "Championship trophy presentation",
    caption: "Championship moments",
  },
  {
    src: "/media/curated/06-dunk-attempt.jpg",
    alt: "Player attempting a dunk",
    caption: "Highlight reel",
  },
  {
    src: "/media/curated/07-scorers-table.jpg",
    alt: "Scorers table and officials at Super6",
    caption: "Professional setup",
  },
  {
    src: "/media/curated/08-warmup-layups.jpg",
    alt: "Teams warming up with layup lines",
    caption: "Pre-game warmups",
  },
  {
    src: "/media/curated/09-defensive-stop.jpg",
    alt: "Defensive stop in traffic",
    caption: "Defense wins",
  },
  {
    src: "/media/curated/10-postgame-handshake.jpg",
    alt: "Teams shaking hands after game",
    caption: "Sportsmanship",
  },
  {
    src: "/media/curated/11-gym-overview.jpg",
    alt: "Aerial view of Super6 gym with multiple courts",
    caption: "Multi-court venues",
  },
  {
    src: "/media/curated/12-player-celebration.jpg",
    alt: "Players celebrating after a win",
    caption: "The feeling of winning",
  },
];

export default function GalleryPage() {
  return (
    <main>
      {/* Hero */}
      <section className="page-hero">
        <Image
          src="/media/curated/11-gym-overview.jpg"
          alt=""
          fill
          priority
          quality={92}
          sizes="100vw"
          aria-hidden="true"
        />
        <div className="container-xl">
          <p
            className="editorial-eyebrow"
            style={{ color: "var(--cream)", opacity: 0.85 }}
          >
            Gallery · Super6 Events
          </p>
          <h1>
            The courts.
            <br />
            <em>The moments.</em>
          </h1>
        </div>
      </section>

      {/* Photo grid */}
      <section style={{ padding: "5rem 0", background: "var(--cream)" }}>
        <div className="container-xl">
          <div
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(auto-fill, minmax(300px, 1fr))",
              gap: "1rem",
            }}
          >
            {photos.map((photo, i) => (
              <div
                key={i}
                style={{
                  position: "relative",
                  aspectRatio: "4/3",
                  overflow: "hidden",
                  borderRadius: "4px",
                  background: "#1a1a1a",
                }}
              >
                <Image
                  src={photo.src}
                  alt={photo.alt}
                  fill
                  sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
                  style={{ objectFit: "cover" }}
                />
                <div
                  style={{
                    position: "absolute",
                    bottom: 0,
                    left: 0,
                    right: 0,
                    padding: "0.75rem 1rem",
                    background:
                      "linear-gradient(transparent, rgba(0,0,0,0.65))",
                    color: "var(--cream)",
                    fontSize: "0.8rem",
                    fontWeight: 600,
                    letterSpacing: "0.05em",
                    textTransform: "uppercase",
                  }}
                >
                  {photo.caption}
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CTA */}
      <section
        style={{
          background: "var(--s6-black)",
          padding: "5rem 0",
          textAlign: "center",
        }}
      >
        <div className="container-xl" style={{ maxWidth: "560px" }}>
          <h2 style={{ color: "var(--cream)", marginBottom: "1rem" }}>
            Your team belongs here.
          </h2>
          <p
            style={{
              color: "var(--cream)",
              opacity: 0.7,
              fontSize: "1.05rem",
              marginBottom: "2rem",
            }}
          >
            Ten events. Four venues. All season long.
          </p>
          <Link
            href="/register"
            className="btn-primary"
            style={{ fontSize: "1rem", padding: "0.9rem 2rem" }}
          >
            Register Your Team
          </Link>
        </div>
      </section>
    </main>
  );
}
