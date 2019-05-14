import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Video } from '../_models/video';
import { VideosService } from '../_services/videos.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable()
export class VideoListResolver implements Resolve<Video[]> {
    constructor(private videosService: VideosService, private router: Router,
        private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Video[]> {
         return this.videosService.getVideos().pipe(
             catchError(error => {
                 this.alertify.error('Problem retrieving data');
                 this.router.navigate(['/home']);
                 return of(null);
             })
         );
    }
}
