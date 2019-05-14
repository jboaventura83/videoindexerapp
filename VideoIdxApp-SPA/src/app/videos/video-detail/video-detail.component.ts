import { Component, OnInit, Input } from '@angular/core';
import { Video } from 'src/app/_models/video';
import { VideosService } from 'src/app/_services/videos.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeUrl} from '@angular/platform-browser';

@Component({
  selector: 'app-video-detail',
  templateUrl: './video-detail.component.html',
  styleUrls: ['./video-detail.component.css']
})
export class VideoDetailComponent implements OnInit {
  video: Video;

  constructor(private videosService: VideosService, private alertify: AlertifyService,
    private route: ActivatedRoute, private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.video = data['video'];
    });
  }

  getlinkEmbedPlayerUrl(): SafeUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(this.video.embedPlayerUrl);
  }

  getlinkEmbedInsightsUrl(): SafeUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(this.video.embedInsightsUrl);
  }

}
