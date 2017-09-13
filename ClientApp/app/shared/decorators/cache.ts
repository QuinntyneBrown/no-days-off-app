import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/share';
import 'rxjs/add/observable/of';


let cachedData: { [key: string]: { date?: Date, observable: Observable<any>, data?: any } } = {};

export function Cache(propertyKey?: string, arg?: number) {
    return (_: Object, functionName: string, descriptor: TypedPropertyDescriptor<any>) => {
        const originalMethod = descriptor.value;
        descriptor.value = function (...args: any[]) {
            const key = getCacheKey(functionName, propertyKey, args, arg || 0);
            let cache = cachedData[key];
            
            if (cache && cache.data) {
                return Observable.of(cache.data);
            } else if (cache && cache.observable) {
                return cache.observable;
            } else {
                cache = {
                    observable: originalMethod.apply(this, args)
                        .map(r => {
                            delete cache.observable;
                            cache.data = r;
                            return cache.data;
                        })
                        .do(null, () => {
                            delete cachedData[key];
                        })
                        .share()
                };
                cachedData[key] = cache;
                return cache.observable;
            }
        };
        return descriptor;
    };
}

export function ClearCache(functionName: string, propertyKey?: string, arg?: number) {
    return (_: Object, __: string, descriptor: TypedPropertyDescriptor<any>) => {
        const originalMethod = descriptor.value;
        descriptor.value = function (...args: any[]) {
            if (functionName === 'clearAllCachedData') {
                cachedData = {};
            } else if (functionName === 'clearAllFunction' && propertyKey) {
                for (const key in cachedData) {
                    if (key.startsWith(propertyKey + '+')) {
                        delete cachedData[key];
                    }
                }
            } else {
                const key = getCacheKey(functionName, propertyKey, args, arg || 0);
                delete cachedData[key];
            }
            return originalMethod.apply(this, args);
        };
        return descriptor;
    };
}

function getCacheKey(functionName: string, propertyName: string, args: any[], arg: number): string {
    let key = `${functionName}+`;
    if (propertyName && args && args.length >= arg && args[arg][propertyName]) {
        key += args[arg][propertyName];
    } else if (args && args.length >= arg && typeof args[arg] === 'string') {
        key += args[arg];
    } else if (args && args.length > 1) {
        key += JSON.stringify(args);
    }
    return key;
}