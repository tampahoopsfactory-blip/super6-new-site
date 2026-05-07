"use client";

import {
  ShieldCheck,
  ScanSearch,
  Users,
  AlertTriangle,
  BadgeCheck,
  type LucideIcon,
} from "lucide-react";
import type { SecurityIcon } from "../security-data";

const REGISTRY: Record<SecurityIcon, LucideIcon> = {
  "shield-check": ShieldCheck,
  "scan-search": ScanSearch,
  users: Users,
  "alert-triangle": AlertTriangle,
  "badge-check": BadgeCheck,
};

type Props = {
  name: SecurityIcon;
  size?: number;
  strokeWidth?: number;
  className?: string;
};

export default function SectionIcon({
  name,
  size = 18,
  strokeWidth = 2,
  className,
}: Props) {
  const Icon = REGISTRY[name] ?? ShieldCheck;
  return (
    <Icon
      size={size}
      strokeWidth={strokeWidth}
      className={className}
      aria-hidden="true"
    />
  );
}
