import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {
    storage:any = localStorage;

    SetItem(key:string,value:string){
        this.storage.setItem(key,value);
    }

    GetItem(key:string):string{
        return this.storage.getItem(key);
    }

    RemoveItem(key:string){
        this.storage.removeItem(key);
    }
    
    Clear(){        
        this.storage.clear();
    }
}