import { Component, OnInit } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { VideosService } from '../_services/videos.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  constructor(private alertify: AlertifyService, private videosService: VideosService) { }

  ngOnInit() {
    this.videosService.geAccessTokenVideoIndexer().subscribe((resp: any) => {
      if (resp) {
        this.alertify.success('Token Video Indexer retornado com sucesso!');
        localStorage.setItem('token-video-indexer', resp);
      }
    }, error => {
      this.alertify.error(error);
    });
  }

}
