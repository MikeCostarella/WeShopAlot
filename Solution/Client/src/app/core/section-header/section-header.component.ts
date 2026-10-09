import { Component, ChangeDetectionStrategy } from '@angular/core';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
    selector: 'app-section-header',
    templateUrl: './section-header.component.html',
    styleUrls: ['./section-header.component.scss'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class SectionHeaderComponent {

  constructor(public bcService: BreadcrumbService) {}

}
