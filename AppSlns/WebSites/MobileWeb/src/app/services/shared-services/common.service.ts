import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class CommonService {
  private dataSource = new BehaviorSubject<boolean>(false);
  private timerReset = new BehaviorSubject<boolean>(false);
  private IsSpanishLang = new BehaviorSubject<boolean>(false);
  data = this.dataSource.asObservable();
  timer = this.timerReset.asObservable();
  isLanguageSpanish = this.IsSpanishLang.asObservable();
  SetLoggedIn(value: boolean) {
    this.dataSource.next(value);
  }
  ResetTimer(value: boolean) {
    this.timerReset.next(value);
  }
  SetSpanish(value: boolean) {
    this.IsSpanishLang.next(value);
  }
  constructor() {}
}
