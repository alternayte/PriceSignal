import { Skeleton } from '@/components/ui/skeleton';

export function SkeletonRule() {
  return (
    <div className="flex items-center space-x-4">
      <div className="flex -space-x-1">
        <Skeleton className="h-6 w-6 rounded-full" />
        <Skeleton className="h-6 w-6 rounded-full" />
      </div>
      <div className="space-y-2">
        <Skeleton className="h-4 w-[250px]" />
        <Skeleton className="h-4 w-[200px]" />
      </div>
    </div>
  );
}
