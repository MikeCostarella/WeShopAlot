// Runs the Angular CLI with BUILD_TIME set to the current time, so the footer and the menu's About
// section can show when this build was made (src/app/core/build-info.ts reads it).
//   npm start      -> node scripts/ng.mjs serve
//   npm run build  -> node scripts/ng.mjs build
import { spawnSync } from 'node:child_process';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const client = join(dirname(fileURLToPath(import.meta.url)), '..');
const ng = join(client, 'node_modules', '@angular', 'cli', 'bin', 'ng.js');
const buildTime = JSON.stringify(new Date().toISOString());

const result = spawnSync(process.execPath, [ng, ...process.argv.slice(2), `--define=BUILD_TIME=${buildTime}`], {
  cwd: client,
  stdio: 'inherit'
});
process.exit(result.status ?? 1);
