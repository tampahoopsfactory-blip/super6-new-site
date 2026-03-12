"use client";

import { useState } from "react";
import Image from "next/image";
import { MapPin, Phone, Mail, Clock, Send } from "lucide-react";
import { siteConfig } from "@/data/site";

export default function ContactPage() {
  const [formState, setFormState] = useState({
    name: "",
    email: "",
    phone: "",
    subject: "general",
    message: "",
  });
  const [submitted, setSubmitted] = useState(false);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitted(true);
  };

  return (
    <>
      {/* Hero */}
      <section className="relative bg-tm-black pt-32 pb-20">
        <div className="absolute inset-0">
          <Image
            src="/media/lifestyle/coach-strategy.jpg"
            alt="Super6 team coordination"
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
            Get in Touch
          </p>
          <h1
            className="mb-4 text-4xl font-semibold tracking-tight text-white md:text-6xl"

          >
            Contact Us
          </h1>
          <p className="max-w-xl text-base text-white/60 leading-relaxed">
            Have questions about registration, venues, or partnerships?
            We&apos;d love to hear from you.
          </p>
        </div>
      </section>

      {/* Contact Section */}
      <section className="tm-section bg-tm-bg">
        <div className="tm-container">
          <div className="grid gap-12 lg:grid-cols-5">
            {/* Contact Info */}
            <div className="lg:col-span-2">
              <h2
                className="mb-6 text-xl font-semibold tracking-tight text-tm-body"
    
              >
                Contact Information
              </h2>
              <div className="space-y-6">
                <div className="flex items-start gap-4">
                  <div className="flex h-10 w-10 shrink-0 items-center justify-center bg-tm-bg-alt">
                    <MapPin size={20} className="text-s6-orange" />
                  </div>
                  <div>
                    <p className="text-sm font-semibold text-tm-body">
                      Main Office
                    </p>
                    <p className="text-sm text-tm-muted">
                      12177 S Orange Blossom Trail
                      <br />
                      Orlando, FL 32837
                    </p>
                  </div>
                </div>

                <div className="flex items-start gap-4">
                  <div className="flex h-10 w-10 shrink-0 items-center justify-center bg-tm-bg-alt">
                    <Phone size={20} className="text-s6-orange" />
                  </div>
                  <div>
                    <p className="text-sm font-semibold text-tm-body">Phone</p>
                    <a
                      href={`tel:${siteConfig.phone}`}
                      className="text-sm text-tm-muted hover:text-s6-orange transition-colors"
                    >
                      {siteConfig.phone}
                    </a>
                  </div>
                </div>

                <div className="flex items-start gap-4">
                  <div className="flex h-10 w-10 shrink-0 items-center justify-center bg-tm-bg-alt">
                    <Mail size={20} className="text-s6-orange" />
                  </div>
                  <div>
                    <p className="text-sm font-semibold text-tm-body">Email</p>
                    <a
                      href={`mailto:${siteConfig.email}`}
                      className="text-sm text-tm-muted hover:text-s6-orange transition-colors"
                    >
                      {siteConfig.email}
                    </a>
                  </div>
                </div>

                <div className="flex items-start gap-4">
                  <div className="flex h-10 w-10 shrink-0 items-center justify-center bg-tm-bg-alt">
                    <Clock size={20} className="text-s6-orange" />
                  </div>
                  <div>
                    <p className="text-sm font-semibold text-tm-body">
                      Tournament Weekends
                    </p>
                    <p className="text-sm text-tm-muted">
                      Check schedule for dates and times
                    </p>
                  </div>
                </div>
              </div>

              {/* Map embed placeholder */}
              <div className="mt-8 overflow-hidden border border-tm-border">
                <div className="relative aspect-[4/3] bg-tm-bg-alt">
                  <Image
                    src="/media/locations/court-branding.jpg"
                    alt="Super6 Orlando venue"
                    fill
                    className="object-cover"
                  />
                  <div className="absolute inset-0 flex items-center justify-center bg-black/40">
                    <a
                      href="https://www.google.com/maps/place/12177+S+Orange+Blossom+Trail,+Orlando,+FL+32837"
                      target="_blank"
                      rel="noopener noreferrer"
                      className="tm-btn tm-btn-white tm-btn-pill text-xs"
                    >
                      Open in Google Maps
                    </a>
                  </div>
                </div>
              </div>
            </div>

            {/* Contact Form */}
            <div className="lg:col-span-3">
              <div className="border border-tm-border bg-tm-bg-alt p-8">
                <h2
                  className="mb-6 text-xl font-semibold tracking-tight text-tm-body"
      
                >
                  Send a Message
                </h2>

                {submitted ? (
                  <div className="py-12 text-center">
                    <div className="mx-auto mb-4 flex h-16 w-16 items-center justify-center bg-green-900/30">
                      <Send size={24} className="text-green-600" />
                    </div>
                    <h3
                      className="mb-2 text-lg font-medium tracking-tight text-tm-body"
          
                    >
                      Message Sent!
                    </h3>
                    <p className="text-sm text-tm-muted">
                      We&apos;ll get back to you as soon as possible.
                    </p>
                  </div>
                ) : (
                  <form onSubmit={handleSubmit} className="space-y-6">
                    <div className="grid gap-6 sm:grid-cols-2">
                      <div>
                        <label
                          htmlFor="name"
                          className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                        >
                          Full Name *
                        </label>
                        <input
                          type="text"
                          id="name"
                          required
                          value={formState.name}
                          onChange={(e) =>
                            setFormState({ ...formState, name: e.target.value })
                          }
                          className="w-full border border-tm-border bg-tm-bg px-4 py-3 text-sm text-tm-body placeholder:text-tm-light focus:border-s6-orange focus:outline-none"
                          placeholder="Your name"
                        />
                      </div>
                      <div>
                        <label
                          htmlFor="email"
                          className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                        >
                          Email *
                        </label>
                        <input
                          type="email"
                          id="email"
                          required
                          value={formState.email}
                          onChange={(e) =>
                            setFormState({ ...formState, email: e.target.value })
                          }
                          className="w-full border border-tm-border bg-tm-bg px-4 py-3 text-sm text-tm-body placeholder:text-tm-light focus:border-s6-orange focus:outline-none"
                          placeholder="your@email.com"
                        />
                      </div>
                    </div>

                    <div className="grid gap-6 sm:grid-cols-2">
                      <div>
                        <label
                          htmlFor="phone"
                          className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                        >
                          Phone
                        </label>
                        <input
                          type="tel"
                          id="phone"
                          value={formState.phone}
                          onChange={(e) =>
                            setFormState({ ...formState, phone: e.target.value })
                          }
                          className="w-full border border-tm-border bg-tm-bg px-4 py-3 text-sm text-tm-body placeholder:text-tm-light focus:border-s6-orange focus:outline-none"
                          placeholder="(555) 123-4567"
                        />
                      </div>
                      <div>
                        <label
                          htmlFor="subject"
                          className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                        >
                          Subject
                        </label>
                        <select
                          id="subject"
                          value={formState.subject}
                          onChange={(e) =>
                            setFormState({
                              ...formState,
                              subject: e.target.value,
                            })
                          }
                          className="w-full border border-tm-border bg-tm-bg px-4 py-3 text-sm text-tm-body focus:border-s6-orange focus:outline-none"
                        >
                          <option value="general">General Inquiry</option>
                          <option value="registration">
                            Team Registration
                          </option>
                          <option value="venue">Venue Partnership</option>
                          <option value="sponsorship">Sponsorship</option>
                          <option value="media">Media / Press</option>
                        </select>
                      </div>
                    </div>

                    <div>
                      <label
                        htmlFor="message"
                        className="mb-2 block text-xs font-medium tracking-tight text-tm-body"
                      >
                        Message *
                      </label>
                      <textarea
                        id="message"
                        required
                        rows={5}
                        value={formState.message}
                        onChange={(e) =>
                          setFormState({
                            ...formState,
                            message: e.target.value,
                          })
                        }
                        className="w-full border border-tm-border bg-tm-bg px-4 py-3 text-sm text-tm-body placeholder:text-tm-light focus:border-s6-orange focus:outline-none"
                        placeholder="Tell us how we can help..."
                      />
                    </div>

                    <button type="submit" className="tm-btn tm-btn-black">
                      Send Message
                    </button>
                  </form>
                )}
              </div>
            </div>
          </div>
        </div>
      </section>
    </>
  );
}
