"use client";

import { useState } from "react";
import Image from "next/image";
import Link from "next/link";
import { Check, ArrowRight, Shield, Trophy, Scale } from "lucide-react";
import { registrationTiers, locations, siteConfig } from "@/data/site";
import { cn } from "@/lib/utils";

type FormData = {
  teamName: string;
  coachName: string;
  email: string;
  phone: string;
  division: string;
  location: string;
  tier: string;
  players: string;
  notes: string;
};

export default function RegisterPage() {
  const [step, setStep] = useState<"pricing" | "form" | "confirmation">(
    "pricing"
  );
  const [selectedTier, setSelectedTier] = useState<string>("");
  const [formData, setFormData] = useState<FormData>({
    teamName: "",
    coachName: "",
    email: "",
    phone: "",
    division: "",
    location: "orlando",
    tier: "",
    players: "",
    notes: "",
  });

  const handleTierSelect = (tierId: string) => {
    if (tierId === "club-package") {
      window.location.href = "/contact";
      return;
    }
    setSelectedTier(tierId);
    setFormData({ ...formData, tier: tierId });
    setStep("form");
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setStep("confirmation");
  };

  return (
    <>
      {/* Hero */}
      <section className="relative bg-tm-black pt-32 pb-16">
        <div className="absolute inset-0">
          <Image
            src="/media/hero/hero-huddle.jpg"
            alt="Team huddle at Super6 tournament"
            fill
            className="object-cover opacity-20"
            priority
          />
        </div>
        <div className="absolute inset-0 bg-gradient-to-b from-black/60 to-tm-black" />
        <div className="tm-container relative">
          <p
            className="mb-4 text-xs font-medium tracking-widest uppercase text-white/60"

          >
            2026 Season
          </p>
          <h1
            className="mb-4 text-4xl font-semibold tracking-tight text-white md:text-6xl"

          >
            Register Your Team
          </h1>
          <p className="max-w-xl text-base text-white/60 leading-relaxed">
            Secure your team&apos;s spot in the Southeast&apos;s most
            competitive youth basketball tournament series.
          </p>
        </div>
      </section>

      {/* Pricing Step */}
      {step === "pricing" && (
        <>
          {/* Trust Badges */}
          <section className="border-b border-tm-border bg-tm-bg py-8">
            <div className="tm-container">
              <div className="grid grid-cols-1 gap-6 sm:grid-cols-3">
                <div className="flex items-center gap-3 text-center sm:text-left">
                  <Shield size={24} className="shrink-0 text-s6-orange" />
                  <div>
                    <p className="text-sm font-semibold text-tm-body">
                      Secure Payment
                    </p>
                    <p className="text-xs text-tm-light">
                      Powered by Stripe
                    </p>
                  </div>
                </div>
                <div className="flex items-center gap-3 text-center sm:text-left">
                  <Trophy size={24} className="shrink-0 text-s6-orange" />
                  <div>
                    <p className="text-sm font-semibold text-tm-body">
                      3+ Games Guaranteed
                    </p>
                    <p className="text-xs text-tm-light">
                      Per tournament weekend
                    </p>
                  </div>
                </div>
                <div className="flex items-center gap-3 text-center sm:text-left">
                  <Scale size={24} className="shrink-0 text-s6-orange" />
                  <div>
                    <p className="text-sm font-semibold text-tm-body">
                      NFHS Officials
                    </p>
                    <p className="text-xs text-tm-light">
                      Certified referees at every game
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </section>

          {/* Pricing Cards */}
          <section className="tm-section bg-tm-bg-warm">
            <div className="tm-container">
              <div className="mb-12 text-center">
                <h2
                  className="mb-4 text-2xl md:text-3xl font-semibold tracking-tight text-tm-body"
      
                >
                  Choose Your Plan
                </h2>
                <p className="mx-auto max-w-lg text-sm text-tm-muted">
                  Select the registration tier that fits your team or club.
                </p>
              </div>

              <div className="grid gap-8 md:grid-cols-3">
                {registrationTiers.map((tier) => (
                  <div
                    key={tier.id}
                    className={cn(
                      "relative flex flex-col overflow-hidden bg-tm-bg-alt border border-tm-border p-8 transition-shadow",
                      tier.popular
                        ? "shadow-xl ring-2 ring-s6-orange"
                        : "shadow-sm hover:shadow-lg"
                    )}
                  >
                    {tier.popular && (
                      <div className="absolute right-0 top-0 bg-s6-orange px-4 py-1.5 text-[10px] font-bold tracking-[2px] text-white uppercase">
                        Most Popular
                      </div>
                    )}

                    <h3
                      className="mb-2 text-lg font-medium tracking-tight text-tm-body"
          
                    >
                      {tier.name}
                    </h3>
                    <div className="mb-1">
                      <span className="text-4xl font-bold text-tm-body">
                        {tier.priceLabel}
                      </span>
                    </div>
                    <p className="mb-6 text-xs text-tm-light">
                      {tier.period}
                    </p>

                    <ul className="mb-8 flex-1 space-y-3">
                      {tier.features.map((feature) => (
                        <li
                          key={feature}
                          className="flex items-start gap-3 text-sm text-tm-muted"
                        >
                          <Check
                            size={16}
                            className="mt-0.5 shrink-0 text-s6-orange"
                          />
                          {feature}
                        </li>
                      ))}
                    </ul>

                    <button
                      onClick={() => handleTierSelect(tier.id)}
                      className={cn(
                        "tm-btn w-full",
                        tier.popular ? "tm-btn-black" : "tm-btn-outline"
                      )}
                    >
                      {tier.cta}
                      <ArrowRight size={16} />
                    </button>
                  </div>
                ))}
              </div>
            </div>
          </section>
        </>
      )}

      {/* Registration Form Step */}
      {step === "form" && (
        <section className="tm-section bg-tm-bg">
          <div className="tm-container max-w-3xl">
            <button
              onClick={() => setStep("pricing")}
              className="mb-8 text-sm font-medium text-tm-muted hover:text-white transition-colors"
            >
              &larr; Back to pricing
            </button>

            <div className="mb-8 bg-tm-bg-warm p-4">
              <p className="text-sm">
                <span className="font-semibold">Selected plan:</span>{" "}
                {registrationTiers.find((t) => t.id === selectedTier)?.name} —{" "}
                {registrationTiers.find((t) => t.id === selectedTier)
                  ?.priceLabel}
              </p>
            </div>

            <h2
              className="mb-6 text-xl md:text-2xl font-semibold tracking-tight text-tm-body"
  
            >
              Team Information
            </h2>

            <form onSubmit={handleSubmit} className="space-y-6">
              <div className="grid gap-6 sm:grid-cols-2">
                <div>
                  <label
                    htmlFor="teamName"
                    className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                  >
                    Team / Club Name *
                  </label>
                  <input
                    type="text"
                    id="teamName"
                    required
                    value={formData.teamName}
                    onChange={(e) =>
                      setFormData({ ...formData, teamName: e.target.value })
                    }
                    className="w-full border border-tm-border bg-tm-bg-alt px-4 py-3 text-sm text-tm-body placeholder:text-tm-light focus:border-s6-orange focus:outline-none"
                    placeholder="Team name"
                  />
                </div>
                <div>
                  <label
                    htmlFor="coachName"
                    className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                  >
                    Head Coach Name *
                  </label>
                  <input
                    type="text"
                    id="coachName"
                    required
                    value={formData.coachName}
                    onChange={(e) =>
                      setFormData({ ...formData, coachName: e.target.value })
                    }
                    className="w-full border border-tm-border bg-tm-bg-alt px-4 py-3 text-sm text-tm-body placeholder:text-tm-light focus:border-s6-orange focus:outline-none"
                    placeholder="Coach full name"
                  />
                </div>
              </div>

              <div className="grid gap-6 sm:grid-cols-2">
                <div>
                  <label
                    htmlFor="regEmail"
                    className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                  >
                    Email *
                  </label>
                  <input
                    type="email"
                    id="regEmail"
                    required
                    value={formData.email}
                    onChange={(e) =>
                      setFormData({ ...formData, email: e.target.value })
                    }
                    className="w-full border border-tm-border bg-tm-bg-alt px-4 py-3 text-sm text-tm-body placeholder:text-tm-light focus:border-s6-orange focus:outline-none"
                    placeholder="coach@email.com"
                  />
                </div>
                <div>
                  <label
                    htmlFor="regPhone"
                    className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                  >
                    Phone *
                  </label>
                  <input
                    type="tel"
                    id="regPhone"
                    required
                    value={formData.phone}
                    onChange={(e) =>
                      setFormData({ ...formData, phone: e.target.value })
                    }
                    className="w-full border border-tm-border bg-tm-bg-alt px-4 py-3 text-sm text-tm-body placeholder:text-tm-light focus:border-s6-orange focus:outline-none"
                    placeholder="(555) 123-4567"
                  />
                </div>
              </div>

              <div className="grid gap-6 sm:grid-cols-2">
                <div>
                  <label
                    htmlFor="division"
                    className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                  >
                    Division / Grade *
                  </label>
                  <select
                    id="division"
                    required
                    value={formData.division}
                    onChange={(e) =>
                      setFormData({ ...formData, division: e.target.value })
                    }
                    className="w-full border border-tm-border bg-tm-bg-alt px-4 py-3 text-sm text-tm-body focus:border-s6-orange focus:outline-none"
                  >
                    <option value="">Select division</option>
                    <option value="3rd">3rd Grade</option>
                    <option value="4th">4th Grade</option>
                    <option value="5th">5th Grade</option>
                    <option value="6th">6th Grade</option>
                    <option value="7th">7th Grade</option>
                    <option value="8th">8th Grade</option>
                    <option value="9th">9th Grade (Freshman)</option>
                    <option value="10th">10th Grade (Sophomore)</option>
                    <option value="11th">11th Grade (Junior)</option>
                    <option value="12th">12th Grade (Senior)</option>
                  </select>
                </div>
                <div>
                  <label
                    htmlFor="location"
                    className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                  >
                    Preferred Location *
                  </label>
                  <select
                    id="location"
                    required
                    value={formData.location}
                    onChange={(e) =>
                      setFormData({ ...formData, location: e.target.value })
                    }
                    className="w-full border border-tm-border bg-tm-bg-alt px-4 py-3 text-sm text-tm-body focus:border-s6-orange focus:outline-none"
                  >
                    {locations
                      .filter((l) => !l.comingSoon)
                      .map((loc) => (
                        <option key={loc.slug} value={loc.slug}>
                          {loc.city}, {loc.state}
                        </option>
                      ))}
                  </select>
                </div>
              </div>

              <div>
                <label
                  htmlFor="players"
                  className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                >
                  Number of Players on Roster
                </label>
                <input
                  type="number"
                  id="players"
                  min={5}
                  max={15}
                  value={formData.players}
                  onChange={(e) =>
                    setFormData({ ...formData, players: e.target.value })
                  }
                  className="w-full border border-tm-border bg-tm-bg-alt px-4 py-3 text-sm text-tm-body placeholder:text-tm-light focus:border-s6-orange focus:outline-none"
                  placeholder="e.g. 10"
                />
              </div>

              <div>
                <label
                  htmlFor="notes"
                  className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                >
                  Additional Notes
                </label>
                <textarea
                  id="notes"
                  rows={3}
                  value={formData.notes}
                  onChange={(e) =>
                    setFormData({ ...formData, notes: e.target.value })
                  }
                  className="w-full border border-tm-border bg-tm-bg-alt px-4 py-3 text-sm text-tm-body placeholder:text-tm-light focus:border-s6-orange focus:outline-none"
                  placeholder="Any special requests or questions..."
                />
              </div>

              <div className="flex flex-col gap-4 pt-4 sm:flex-row">
                <button type="submit" className="tm-btn tm-btn-black flex-1">
                  Proceed to Payment
                  <ArrowRight size={16} />
                </button>
              </div>

              <p className="text-xs text-tm-light">
                By registering, you agree to Super6&apos;s tournament rules and
                code of conduct. Payment is processed securely via Stripe. All
                teams must have valid team insurance.
              </p>
            </form>
          </div>
        </section>
      )}

      {/* Confirmation Step */}
      {step === "confirmation" && (
        <section className="tm-section bg-tm-bg">
          <div className="tm-container max-w-2xl text-center">
            <div className="mx-auto mb-6 flex h-20 w-20 items-center justify-center bg-green-900/30">
              <Check size={36} className="text-green-600" />
            </div>
            <h2
              className="mb-4 text-2xl md:text-3xl font-semibold tracking-tight text-tm-body"
  
            >
              Registration Submitted!
            </h2>
            <p className="mb-4 text-sm text-tm-muted">
              Thank you, <strong>{formData.coachName}</strong>! Your
              registration for <strong>{formData.teamName}</strong> has been
              received.
            </p>
            <p className="mb-8 text-xs text-tm-light">
              You&apos;ll receive a confirmation email at{" "}
              <strong>{formData.email}</strong> with payment instructions and
              next steps. If you have questions, call us at {siteConfig.phone}.
            </p>
            <div className="flex flex-col items-center justify-center gap-4 sm:flex-row">
              <Link href="/" className="tm-btn tm-btn-black">
                Back to Home
              </Link>
              <Link
                href="/locations"
                className="text-sm font-semibold text-tm-muted hover:text-white transition-colors"
              >
                View Locations &rarr;
              </Link>
            </div>
          </div>
        </section>
      )}
    </>
  );
}
