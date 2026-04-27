"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
import Image from "next/image";
import { usePathname } from "next/navigation";
import { Menu, X } from "lucide-react";
import { announcements } from "@/data/site";

const navLinks = [
  { label: "Our Mission", href: "/about" },
  { label: "Tournaments", href: "/register" },
  { label: "Locations", href: "/locations" },
  { label: "Rules", href: "/rules" },
  { label: "Contact", href: "/contact" },
];

export default function Navigation() {
  const pathname = usePathname();
  const [mobileOpen, setMobileOpen] = useState(false);
  const [scrolled, setScrolled] = useState(false);
  const [announcementIdx, setAnnouncementIdx] = useState(0);

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
        <Link href="/" className="nav-logo">
          <Image
            src="/media/logos/super6-mark.png"
            alt="Super 6"
            width={32}
            height={32}
            className="nav-logo-img"
            style={{ height: 30, width: "auto" }}
          />
          <span className="nav-logo-text">Super 6</span>
        </Link>

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
        </ul>

        <div className="nav-right">
          <Link href="/register" className="nav-cta">
            Register Now
          </Link>
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
          <Link href="/" className="nav-logo" onClick={() => setMobileOpen(false)}>
            <Image
              src="/media/logos/logo-small-transparent.png"
              alt="Super 6"
              width={36}
              height={36}
              className="h-8 w-8 object-contain"
            />
            <span className="nav-logo-text">Super 6</span>
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
          <Link href="/register" className="mobile-menu-cta" onClick={() => setMobileOpen(false)}>
            Register Your Team
          </Link>
        </div>
      </div>
    </>
  );
}
