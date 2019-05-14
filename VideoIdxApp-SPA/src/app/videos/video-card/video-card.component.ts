import { Component, OnInit, Input } from '@angular/core';
import { Video } from 'src/app/_models/video';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-video-card',
  templateUrl: './video-card.component.html',
  styleUrls: ['./video-card.component.css']
})
export class VideoCardComponent implements OnInit {
  @Input() video: Video;

  constructor(private sanitizer: DomSanitizer) { }

  ngOnInit() {
  }

  getTrustedImage() {
    return this.sanitizer.bypassSecurityTrustResourceUrl(`data:image/jpg;base64,${this.video.base64ThumbnailImage}`);
  }

}
