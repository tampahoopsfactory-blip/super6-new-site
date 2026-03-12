import type { Metadata } from "next";
import Image from "next/image";
import Link from "next/link";
import { MapPin, ArrowRight, Phone } from "lucide-react";
import { locations, siteConfig } from "@/data/site";

export const metadata: Metadata = {
  title: "Locations",
  description:
    "Find Super6 youth basketball tournaments near you. Venues in Orlando, Clearwater, Tampa, West Palm Beach, and Atlanta (coming soon).",
};

export default function LocationsPage() {
  return (
    <>
      {/* Page Hero */}
      <section className="relative bg-tm-black pt-32 pb-20">
        <div
          className="absolute inset-0 opacity-20"
          style={{
            backgroundImage: "url(/media/hero/hero-crowd.jpg)",
            backgroundSize: "cover",
            backgroundPosition: "center",
          }}
          aria-hidden="true"
        />
        <div className="absolute inset-0 bg-gradient-to-b from-black/80 to-tm-black" />
        <div className="tm-container relative">
          <p
            className="mb-4 text-xs font-medium tracking-widest uppercase text-white/60"

          >
            Where We Play
          </p>
          <h1
            className="mb-4 text-4xl font-semibold tracking-tight text-white md:text-6xl"

          >
            Our Locations
          </h1>
          <p className="max-w-xl text-base text-white/60 leading-relaxed">
            Championship-level youth basketball across Florida and Georgia.
            Find the Super6 venue nearest you.
          </p>
        </div>
      </section>

      {/* Locations Grid */}
      <section className="tm-section bg-tm-bg">
        <div className="tm-container">
          <div className="grid gap-8 md:grid-cols-2 lg:grid-cols-3">
            {locations.map((loc) => (
              <Link
                key={loc.slug}
                href={`/locations/${loc.slug}`}
                className="tm-card-hover group overflow-hidden border border-tm-border"
              >
                <div className="tm-img-zoom relative aspect-[16/10]">
                  <Image
                    src={loc.image}
                    alt={`Super6 ${loc.city} venue`}
                    fill
                    sizes="(max-width: 768px) 100vw, (max-width: 1024px) 50vw, 33vw"
                    className="object-cover"
                  />
                  {loc.comingSoon && (
                    <div className="absolute right-4 top-4 bg-s6-orange px-4 py-1.5 text-xs font-bold tracking-[2px] text-white uppercase">
                      Coming Soon
                    </div>
                  )}
                </div>
                <div className="p-6">
                  <div className="mb-3 flex items-center gap-2">
                    <MapPin size={16} className="text-s6-orange" />
                    <span className="text-xs font-medium tracking-widest text-tm-light uppercase">
                      {loc.city}, {loc.state}
                    </span>
                  </div>
                  <h2
                    className="mb-2 text-lg font-medium tracking-tight text-tm-body group-hover:text-s6-orange transition-colors"
        
                  >
                    {loc.name}
                  </h2>
                  <p className="mb-4 text-sm leading-relaxed text-tm-muted">
                    {loc.description}
                  </p>
                  {loc.address && (
                    <p className="mb-2 text-xs text-tm-light">
                      {loc.address}
                    </p>
                  )}
                  <div className="flex items-center gap-1 text-sm font-semibold text-tm-body">
                    <span>View Details</span>
                    <ArrowRight
                      size={14}
                      className="transition-transform group-hover:translate-x-1"
                    />
                  </div>
                </div>
              </Link>
            ))}
          </div>
        </div>
      </section>

      {/* Contact Banner */}
      <section className="bg-tm-bg-warm py-16">
        <div className="tm-container text-center">
          <h2
            className="mb-4 text-2xl md:text-3xl font-semibold tracking-tight text-tm-body"

          >
            Want to Host a Super6 Tournament?
          </h2>
          <p className="mx-auto mb-8 max-w-lg text-sm text-tm-muted leading-relaxed">
            We&apos;re always looking for great venues. If you have a facility
            that can host championship-level youth basketball, let&apos;s talk.
          </p>
          <div className="flex flex-col items-center justify-center gap-4 sm:flex-row">
            <Link href="/contact" className="tm-btn tm-btn-black tm-btn-pill">
              Get in Touch
            </Link>
            <a
              href={`tel:${siteConfig.phone}`}
              className="flex items-center gap-2 text-sm font-semibold text-tm-muted hover:text-white transition-colors"
            >
              <Phone size={16} />
              {siteConfig.phone}
            </a>
          </div>
        </div>
      </section>
    </>
  );
}
