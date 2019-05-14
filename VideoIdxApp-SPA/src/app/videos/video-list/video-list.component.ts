import { Component, OnInit } from '@angular/core';
import { Video } from 'src/app/_models/video';
import { VideosService } from 'src/app/_services/videos.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-video-list',
  templateUrl: './video-list.component.html',
  styleUrls: ['./video-list.component.css']
})
export class VideoListComponent implements OnInit {
  videos: Video[];

  constructor(private videosService: VideosService, private alertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.videos = data['videos'];
    });
  }

}
