# WeShopAlot design system

Both clients (Angular and WPF) take their colors, fonts, sizes and spacing from one file, **tokens.json**.
Change a value there, regenerate, and both clients pick it up.

## Files

| File | What it is |
| --- | --- |
| `tokens.json` | The design tokens: colors, fonts, font sizes, spacing, layout sizes, corner radii |
| `build-tokens.mjs` | Node script (no packages needed) that turns the tokens into code for each client |
| `logo/pick.svg`, `logo/pick-256.png` | The guitar-pick logo mark |

## Generated files (do not edit by hand)

| Client | File | How it's used |
| --- | --- | --- |
| Angular | `Client/src/styles/_tokens.scss` | `$ws-*` Sass variables; `styles.scss` feeds them into Bootstrap's variables |
| WPF | `WeShopAlot.UI.ClientWPF/Resources/Tokens.xaml` | `{Key}Brush`, `HeadingFont`, `FontSize*`, `Space*`… used by `Styles.xaml` and the views |

## Regenerate after changing tokens.json

PowerShell:

```powershell
cd C:\projects\WeShopAlot\Solution\Client
npm run tokens
```

Then rebuild both clients (`npm run build` for Angular; Build in Visual Studio for WPF).

## Brand

- **Rosewood** `#A3541A` is the primary color (buttons, prices, links); white text on it meets WCAG AA.
- **Sunburst** `#F2B35C` is the accent (basket badge).
- **Ink**, **Slate**, **Maple** and **Fret** are the text, muted text, panel and border colors.
- Headings use **Barlow Condensed**, body text uses **Barlow** (SIL Open Font License). Angular loads them from
  `@fontsource`; WPF compiles the .ttf files in `Fonts/` into the app.

## Shared layout

Both clients use the same frame: a header (menu button, logo, HOME/SHOP, basket badge, sign-in), a hamburger
accordion menu (Shop, My account, View, Links, About), a page band with the title and breadcrumb, and a dark
footer or status bar with the client name, API address and build time. The shop page offers **Cards** and
**List** views, remembered between visits.
