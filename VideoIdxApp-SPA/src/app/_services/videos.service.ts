import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Video } from '../_models/video';

const httpOptions = {
  headers: new HttpHeaders({
    'Authorization': 'Bearer ' + localStorage.getItem('token')
  })
};

@Injectable({
  providedIn: 'root'
})
export class VideosService {
  baseUrl = environment.apiUrl;


  constructor(private http: HttpClient) { }

  geAccessTokenVideoIndexer(): Observable<any> {
    return this.http.get<any>(this.baseUrl + 'videos/auth/', httpOptions);
  }

  getVideos(): Observable<Video[]> {
    return this.http.get<Video[]>(this.baseUrl + 'videos', httpOptions);
  }

  getVideo(id): Observable<Video> {
    return this.http.get<Video>(this.baseUrl + 'videos/' + id, httpOptions);
  }

}
