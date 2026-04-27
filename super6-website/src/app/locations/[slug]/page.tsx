import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import { notFound } from "next/navigation";
import { MapPin, Phone, ExternalLink, Clock, ArrowLeft } from "lucide-react";
import { locations } from "@/data/site";

type Props = {
  params: Promise<{ slug: string }>;
};

export async function generateStaticParams() {
  return locations.map((loc) => ({ slug: loc.slug }));
}

export async function generateMetadata({ params }: Props): Promise<Metadata> {
  const { slug } = await params;
  const location = locations.find((l) => l.slug === slug);
  if (!location) return {};
  return {
    title: `${location.name} - Youth Basketball`,
    description: location.description,
  };
}

export default async function LocationDetailPage({ params }: Props) {
  const { slug } = await params;
  const location = locations.find((l) => l.slug === slug);

  if (!location) notFound();

  return (
    <>
      {/* Hero */}
      <section className="page-hero">
        <Image
          src={location.image}
          alt=""
          fill
          priority
          sizes="100vw"
          aria-hidden="true"
        />
        <div className="container-xl">
          <Link
            href="/locations"
            style={{
              display: "inline-flex",
              alignItems: "center",
              gap: 8,
              fontSize: 13,
              fontFamily: "var(--font-body)",
              color: "rgba(244, 241, 234, 0.7)",
              textDecoration: "none",
              marginBottom: 24,
              letterSpacing: 0.005,
              transition: "color var(--dur-fast) var(--ease)",
            }}
          >
            <ArrowLeft size={14} />
            All Locations
          </Link>

          {location.comingSoon && (
            <div
              style={{
                display: "inline-block",
                marginBottom: 14,
                padding: "5px 14px",
                background: "var(--orange)",
                color: "var(--white)",
                fontFamily: "var(--font-body)",
                fontSize: 11,
                fontWeight: 600,
                letterSpacing: "0.12em",
                textTransform: "uppercase",
                borderRadius: "var(--radius-pill)",
              }}
            >
              Coming Soon
            </div>
          )}

          <p className="editorial-eyebrow" style={{ color: "var(--cream)", opacity: 0.85 }}>
            {location.state === "GA" ? "Georgia" : "Florida"}
          </p>
          <h1>{location.name}</h1>
          <p>{location.description}</p>
        </div>
      </section>

      {/* Details */}
      <section className="tm-section bg-tm-bg">
        <div className="tm-container">
          <div className="grid gap-12 lg:grid-cols-3">
            {/* Main Content */}
            <div className="lg:col-span-2">
              <div className="tm-img-zoom mb-8 overflow-hidden">
                <div className="relative aspect-[16/9]">
                  <Image
                    src={location.image}
                    alt={`Super6 ${location.city} venue interior`}
                    fill
                    sizes="(max-width: 1024px) 100vw, 66vw"
                    className="object-cover"
                  />
                </div>
              </div>

              <h2
                className="mb-4 text-xl md:text-2xl font-semibold tracking-tight text-tm-body"
    
              >
                About This Venue
              </h2>
              <div className="space-y-4 text-sm text-tm-muted leading-relaxed">
                <p>{location.description}</p>
                {!location.comingSoon ? (
                  <>
                    <p>
                      Super6 {location.city} hosts regular tournament weekends
                      featuring competitive bracket play for grades 3-12. Every
                      game is officiated by NFHS-certified referees and
                      tracked with digital scorebooks and professional stats.
                    </p>
                    <h3
                      className="text-lg font-medium tracking-tight text-tm-body pt-4"
          
                    >
                      What to Expect
                    </h3>
                    <ul className="space-y-2 list-none">
                      {[
                        "Professional regulation courts",
                        "Championship atmosphere and court branding",
                        "NFHS-certified referees",
                        "Digital scorebook and stats tracking",
                        "Concessions and spectator seating",
                        "Climate-controlled indoor facility",
                      ].map((item) => (
                        <li key={item} className="flex items-center gap-3">
                          <span className="h-1.5 w-1.5 shrink-0 bg-s6-orange" />
                          {item}
                        </li>
                      ))}
                    </ul>
                  </>
                ) : (
                  <p>
                    We&apos;re bringing the Super6 championship experience to{" "}
                    {location.city}. Stay tuned for venue details, tournament
                    schedules, and registration opening dates. Sign up for
                    notifications to be the first to know.
                  </p>
                )}
              </div>
            </div>

            {/* Sidebar */}
            <div>
              <div className="sticky top-28 space-y-6">
                {/* Info Card */}
                <div className="border border-tm-border bg-tm-bg-warm p-6">
                  <h3
                    className="mb-4 text-sm font-semibold tracking-tight text-tm-body"
        
                  >
                    Venue Details
                  </h3>
                  <ul className="space-y-4">
                    {location.address && (
                      <li className="flex items-start gap-3 text-sm">
                        <MapPin
                          size={18}
                          className="mt-0.5 shrink-0 text-s6-orange"
                        />
                        <span className="text-tm-muted">
                          {location.address}
                        </span>
                      </li>
                    )}
                    {location.phone && (
                      <li>
                        <a
                          href={`tel:${location.phone}`}
                          className="flex items-center gap-3 text-sm text-tm-muted hover:text-s6-orange transition-colors"
                        >
                          <Phone
                            size={18}
                            className="shrink-0 text-s6-orange"
                          />
                          {location.phone}
                        </a>
                      </li>
                    )}
                    <li className="flex items-center gap-3 text-sm text-tm-muted">
                      <Clock size={18} className="shrink-0 text-s6-orange" />
                      {location.comingSoon
                        ? "Hours TBA"
                        : "Tournament weekends — check schedule"}
                    </li>
                    {location.mapUrl && (
                      <li>
                        <a
                          href={location.mapUrl}
                          target="_blank"
                          rel="noopener noreferrer"
                          className="flex items-center gap-3 text-sm font-semibold text-tm-body hover:text-s6-orange transition-colors"
                        >
                          <ExternalLink size={18} className="shrink-0" />
                          Get Directions
                        </a>
                      </li>
                    )}
                  </ul>
                </div>

                {/* CTA Card */}
                <div className="bg-tm-black p-6 text-center">
                  <h3
                    className="mb-3 text-lg font-medium tracking-tight text-white"
        
                  >
                    {location.comingSoon
                      ? "Get Notified"
                      : "Register Your Team"}
                  </h3>
                  <p className="mb-5 text-sm text-white/60">
                    {location.comingSoon
                      ? `Be the first to know when ${location.city} opens for registration.`
                      : `Secure your spot at the next ${location.city} tournament.`}
                  </p>
                  <Link
                    href={location.comingSoon ? "/contact" : "/register"}
                    className="tm-btn tm-btn-white w-full"
                  >
                    {location.comingSoon ? "Contact Us" : "Register Now"}
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    </>
  );
}
