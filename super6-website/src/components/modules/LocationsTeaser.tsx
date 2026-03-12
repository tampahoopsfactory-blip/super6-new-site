import Image from "next/image";
import Link from "next/link";

/* ─── Division Cards — 3-up Nike full-bleed category cards
   Sharp corners, gradient overlay, white pill labels.
   Uses real game photos for maximum visual impact. ─── */

const divisions = [
  {
    label: "Boys Division",
    subtitle: "12th–3rd Grade",
    image: "/media/gallery/G1_24_Defensive_Action.jpg",
    href: "/register",
  },
  {
    label: "Girls Division",
    subtitle: "12th–6th Grade",
    image: "/media/gallery/G1_21_Youth_Dribbling.jpg",
    href: "/register",
  },
  {
    label: "Elite Competitive",
    subtitle: "Top-Tier Play",
    image: "/media/gallery/G1_32_Contest_Layup.jpg",
    href: "/register",
  },
];

export default function DivisionCards() {
  return (
    <section className="section" aria-label="Divisions">
      <div className="container-xl" style={{ textAlign: "center", marginBottom: "48px" }}>
        <p className="section-label">Find Your Competition</p>
        <h2 className="section-heading" style={{ margin: "0 auto" }}>
          Every Level. Every Division.
        </h2>
      </div>
      <div className="container-xl">
        <div style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(300px, 1fr))",
          gap: "4px",
        }}>
          {divisions.map((div) => (
            <Link key={div.label} href={div.href} className="card" style={{ textDecoration: "none" }}>
              <Image
                src={div.image}
                alt={div.label}
                fill
                sizes="(max-width: 768px) 100vw, 33vw"
                className="object-cover"
              />
              <div className="card-overlay" />
              <div className="card-content">
                <p className="card-label">{div.subtitle}</p>
                <h3 className="card-title">{div.label}</h3>
                <span className="btn-hero btn-hero-primary" style={{ height: "48px", padding: "0 28px", fontSize: "14px", fontWeight: 600, letterSpacing: "-0.01em" }}>
                  Register Now
                </span>
              </div>
            </Link>
          ))}
        </div>
      </div>
    </section>
  );
}
