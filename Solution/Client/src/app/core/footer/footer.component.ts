import { ChangeDetectionStrategy, Component } from '@angular/core';
import { environment } from '../../../environments/environment';
import { buildTime } from '../build-info';

/** Which client this is, the API it talks to, and when it was built (the WPF client shows the same in its status bar). */
@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class FooterComponent {
  readonly apiUrl = new URL(environment.apiUrl, document.baseURI).origin;
  readonly buildText = buildTime ? `Built ${buildTime.toLocaleString()}` : 'Development build';
}
