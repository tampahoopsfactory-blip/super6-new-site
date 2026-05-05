"use client";

import { useState, useEffect, useRef } from "react";
import Link from "next/link";
import Image from "next/image";
import { usePathname } from "next/navigation";
import { Menu, X, ChevronDown } from "lucide-react";
import { announcements } from "@/data/site";
import { REGISTER_LINK_PROPS } from "@/lib/links";

const navLinks = [
  { label: "Home", href: "/" },
  { label: "Our Mission", href: "/about" },
  { label: "Tournaments", href: "/register" },
  { label: "Locations", href: "/locations" },
  { label: "Gallery", href: "/gallery" },
  { label: "Coaches", href: "/coaches" },
  { label: "Hiring Officials", href: "/officials" },
  { label: "AI Events", href: "/ai-events" },
  { label: "Rules", href: "/rules" },
  { label: "FAQ", href: "/faq" },
  { label: "Contact", href: "/contact" },
];

const megaMenu = [
  {
    title: "Programs",
    links: [
      { label: "Youth Development · 3rd–5th", href: "/programs#youth" },
      { label: "Travel Teams · 6th–8th", href: "/programs#travel" },
      { label: "School Programs", href: "/programs#school" },
      { label: "Elite Competition · 9th–12th", href: "/programs#elite" },
    ],
  },
  {
    title: "Community",
    links: [
      { label: "For Coaches", href: "/coaches" },
      { label: "Champions", href: "/champions" },
      { label: "News & Updates", href: "/news" },
    ],
  },
  {
    title: "Resources",
    links: [
      { label: "FAQs", href: "/faq" },
      { label: "AI Events", href: "/ai-events" },
      { label: "Rules", href: "/rules" },
    ],
  },
  {
    title: "Connect",
    links: [
      { label: "About Us", href: "/about" },
      { label: "Sponsorships", href: "/sponsors" },
      { label: "Become an Official", href: "/officials" },
    ],
  },
];

export default function Navigation() {
  const pathname = usePathname();
  const [mobileOpen, setMobileOpen] = useState(false);
  const [scrolled, setScrolled] = useState(false);
  const [announcementIdx, setAnnouncementIdx] = useState(0);
  const [megaOpen, setMegaOpen] = useState(false);
  const megaRef = useRef<HTMLLIElement>(null);

  useEffect(() => {
    const onScroll = () => setScrolled(window.scrollY > 10);
    window.addEventListener("scroll", onScroll, { passive: true });
    return () => window.removeEventListener("scroll", onScroll);
  }, []);

  useEffect(() => {
    const timer = setInterval(() => {
      setAnnouncementIdx((i) => (i + 1) % announcements.length);
    }, 5000);
    return () => clearInterval(timer);
  }, []);

  useEffect(() => {
    document.body.style.overflow = mobileOpen ? "hidden" : "";
    return () => { document.body.style.overflow = ""; };
  }, [mobileOpen]);

  // Close mega-menu on outside click or Escape key
  useEffect(() => {
    if (!megaOpen) return;
    const onClick = (e: MouseEvent) => {
      if (megaRef.current && !megaRef.current.contains(e.target as Node)) {
        setMegaOpen(false);
      }
    };
    const onKey = (e: KeyboardEvent) => { if (e.key === "Escape") setMegaOpen(false); };
    document.addEventListener("mousedown", onClick);
    document.addEventListener("keydown", onKey);
    return () => {
      document.removeEventListener("mousedown", onClick);
      document.removeEventListener("keydown", onKey);
    };
  }, [megaOpen]);

  // Close mega-menu when route changes.
  // Deferred via setTimeout to satisfy react-hooks/set-state-in-effect — same
  // pattern used in src/app/faq/_components/FAQClient.tsx.
  useEffect(() => {
    const t = setTimeout(() => setMegaOpen(false), 0);
    return () => clearTimeout(t);
  }, [pathname]);

  const announcement = announcements[announcementIdx];

  return (
    <>
      {/* Announcement bar */}
      <div className="announcement-bar" role="banner">
        <span>{announcement.text}</span>
        <Link href={announcement.link}>{announcement.linkText} →</Link>
      </div>

      {/* Main navigation */}
      <nav className={`nav-main ${pathname === "/" && !scrolled ? "nav-main--transparent" : ""} ${scrolled ? "nav-main--scrolled" : ""}`} role="navigation" aria-label="Main">
        <ul className="nav-links">
          {navLinks.map((link) => (
            <li key={link.href + link.label}>
              <Link
                href={link.href}
                className={pathname === link.href ? "active" : ""}
              >
                {link.label}
              </Link>
            </li>
          ))}
          <li ref={megaRef} className={`nav-mega-trigger ${megaOpen ? "is-open" : ""}`}>
            <button
              type="button"
              className="nav-mega-button"
              aria-haspopup="true"
              aria-expanded={megaOpen}
              onClick={() => setMegaOpen((v) => !v)}
            >
              More <ChevronDown size={14} strokeWidth={2.2} className="nav-mega-chevron" />
            </button>
            {megaOpen && (
              <div className="nav-mega-panel" role="menu">
                <div className="nav-mega-grid">
                  {megaMenu.map((col) => (
                    <div key={col.title} className="nav-mega-col">
                      <p className="nav-mega-col-title">{col.title}</p>
                      <ul>
                        {col.links.map((l) => (
                          <li key={l.href + l.label}>
                            <Link href={l.href} role="menuitem">{l.label}</Link>
                          </li>
                        ))}
                      </ul>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </li>
        </ul>

        <div className="nav-right">
          <button
            className="nav-hamburger"
            onClick={() => setMobileOpen(true)}
            aria-label="Open menu"
          >
            <Menu size={24} strokeWidth={2} />
          </button>
        </div>
      </nav>

      {/* Mobile menu */}
      <div className={`mobile-menu ${mobileOpen ? "mobile-menu--open" : ""}`} role="dialog" aria-modal="true">
        <div className="mobile-menu-header">
          <Link
            href="/"
            className="nav-logo"
            aria-label="Super6 Series LLC"
            onClick={() => setMobileOpen(false)}
          >
            <Image
              src="/media/logos/logo-small-transparent.png"
              alt="Super6 Series LLC"
              width={64}
              height={64}
              className="object-contain"
              style={{ width: 64, height: 64 }}
            />
          </Link>
          <button
            className="mobile-menu-close"
            onClick={() => setMobileOpen(false)}
            aria-label="Close menu"
          >
            <X size={24} strokeWidth={2} />
          </button>
        </div>
        <nav className="mobile-menu-nav">
          <Link href="/" onClick={() => setMobileOpen(false)}>Home</Link>
          {navLinks.map((link) => (
            <Link key={link.href + link.label} href={link.href} onClick={() => setMobileOpen(false)}>
              {link.label}
            </Link>
          ))}
        </nav>
        <div className="mobile-menu-footer">
          <Link
            {...REGISTER_LINK_PROPS}
            className="mobile-menu-cta"
            onClick={() => setMobileOpen(false)}
          >
            Register Your Team
          </Link>
        </div>
      </div>
    </>
  );
}
